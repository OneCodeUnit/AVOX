using System;
using System.Drawing;
using System.Threading.Tasks;
using static System.Math;
using static AVOX2.Calc;

namespace AVOX2
{
    internal static partial class VoxImageBasic
    {
        internal static partial string[] NewVoxImage(string[] F, int size, double area, string code, double D, bool color)
        {
            //Объект для работы с изображением
            Bitmap Bitmap_x = new(size, size, pixelformat);
            Bitmap Bitmap_y = new(size, size, pixelformat);
            Bitmap Bitmap_z = new(size, size, pixelformat);
            Bitmap Bitmap_t = new(size, size, pixelformat);

            //Создание более быстрых битмапов на указателях
            LockBitmap lockBitmap_x = new(Bitmap_x);
            LockBitmap lockBitmap_y = new(Bitmap_y);
            LockBitmap lockBitmap_z = new(Bitmap_z);
            LockBitmap lockBitmap_t = new(Bitmap_t);

            //Создание объекта на основле полученного пути к изображению
            Bitmap CxF = new(F[0], false);
            Bitmap CyF = new(F[1], false);
            Bitmap CzF = new(F[2], false);
            Bitmap CtF = new(F[3], false);

            //Создание более быстрых битмапов на указателях
            LockBitmap lockCxF = new(CxF);
            LockBitmap lockCyF = new(CyF);
            LockBitmap lockCzF = new(CzF);
            LockBitmap lockCtF = new(CtF);

            //Выбор палитры
            double P;
            if (color)
                P = P_RGB;
            else
                P = P_Gray;

            //Размер области, на которой находятся значения функции
            double Xmin = -area;
            double Xmax = area;
            double Ymin = -area;
            double Ymax = area;

            //Сжатие или растяжение области значений до размера выходного изображения
            double stepX = (Xmax - Xmin) / size;
            double stepY = (Ymax - Ymin) / size;

            //Блокировка
            lockBitmap_x.LockBits();
            lockBitmap_y.LockBits();
            lockBitmap_z.LockBits();
            lockBitmap_t.LockBits();

            //Блокировка
            lockCxF.LockBits();
            lockCyF.LockBits();
            lockCzF.LockBits();
            lockCtF.LockBits();

            //Запуск потоков по числу потоков процессора
            int cores = Environment.ProcessorCount;
            Task[] tasks = new Task[cores];

            for (int tasknumber = 0; tasknumber < tasks.Length; tasknumber++)
            {
                //Разделение области вычисления на равные части каждому потоку
                int start = (size / cores) * tasknumber;
                int end = (size / cores) * (tasknumber + 1);
                tasks[tasknumber] = Task.Run(() =>
                {
                    for (int i = start; i < end; i++)
                    {
                        double x1 = Xmin + i * stepX;
                        double x2 = Xmin + (i + 1) * stepX;
                        double x3 = Xmin + i * stepX;

                        for (int j = 0; j < size - 1; j++)
                        {
                            double y1 = Ymin + j * stepY;
                            double y2 = Ymin + j * stepY;
                            double y3 = Ymin + (j + 1) * stepY;

                            //Извлечение численного значения из пикселя
                            int m1 = GetValue(i, j, lockCxF);
                            int m2 = GetValue(i, j, lockCyF);
                            int m3 = GetValue(i, j, lockCzF);
                            int m4 = GetValue(i, j, lockCtF);

                            //Переход от цвета к значениям нормального векторного поля
                            double n1 = (2 * m1 - P) / P;
                            double n2 = (2 * m2 - P) / P;
                            double n3 = (2 * m3 - P) / P;
                            double n4 = (2 * m4 - P) / P;

                            double zf1 = -x1 * (n1 / n3) - y1 * (n2 / n3) - (n4 / n3);
                            double zf2 = -x2 * (n1 / n3) - y2 * (n2 / n3) - (n4 / n3);
                            double zf3 = -x3 * (n1 / n3) - y3 * (n2 / n3) - (n4 / n3);
                            double zf = zf1;

                            double a1, a2, a3, a4;
                            switch (code)
                            {
                                case "pow":
                                    a1 = n1 * Pow(zf, D - 1);
                                    a2 = n2 * Pow(zf, D - 1);
                                    a3 = n3;
                                    a4 = n4 * Pow(zf, D - 1);
                                    break;
                                case "sqrt":
                                    a1 = n1;
                                    a2 = n2;
                                    a3 = n3 * Pow(zf, 1 / D);
                                    a4 = n4;
                                    break;
                                case "number":
                                    a1 = n1 * D;
                                    a2 = n2 * D;
                                    a3 = n3;
                                    a4 = n4 * D;
                                    break;
                                case "abs":
                                    a1 = Abs(n1);
                                    a2 = Abs(n2);
                                    a3 = n3;
                                    a4 = Abs(n4);
                                    break;
                                default:
                                    a1 = n1 * Pow(zf, D - 1);
                                    a2 = n2 * Pow(zf, D - 1);
                                    a3 = n3;
                                    a4 = n4 * Pow(zf, D - 1);
                                    break;
                            }

                            //Нормирование нормой n
                            double n = Sqrt((a1 * a1) + (a2 * a2) + (a3 * a3) + (a4 * a4));
                            n1 = a1 / n;
                            n2 = a2 / n;
                            n3 = a3 / n;
                            n4 = a4 / n;

                            //Преобразование нормального векторного поля в цвет
                            double Cx = (n1 + 1) * P / 2;
                            double Cy = (n2 + 1) * P / 2;
                            double Cz = (n3 + 1) * P / 2;
                            double Ct = (n4 + 1) * P / 2;

                            //Запись цвета в пиксель
                            lockBitmap_x.SetPixel(i, j, SetColor(Cx, color));
                            lockBitmap_y.SetPixel(i, j, SetColor(Cy, color));
                            lockBitmap_z.SetPixel(i, j, SetColor(Cz, color));
                            lockBitmap_t.SetPixel(i, j, SetColor(Ct, color));
                        }
                    }
                });
            }

            //Завершение потоков и снятие блокировки
            Task.WaitAll(tasks);
            lockBitmap_x.UnlockBits();
            lockBitmap_y.UnlockBits();
            lockBitmap_z.UnlockBits();
            lockBitmap_t.UnlockBits();

            lockCxF.UnlockBits();
            lockCyF.UnlockBits();
            lockCzF.UnlockBits();
            lockCtF.UnlockBits();

            //Наименования для выходных файлов и возвращаемых значений
            string x = "1" + code + "Cx." + imageformat;
            string y = "1" + code + "Cy." + imageformat;
            string z = "1" + code + "Cz." + imageformat;
            string t = "1" + code + "Ct." + imageformat;

            //Сохранение изображений в выбранном формате
            Bitmap_x.Save(x, imageformat);
            Bitmap_y.Save(y, imageformat);
            Bitmap_z.Save(z, imageformat);
            Bitmap_t.Save(t, imageformat);

            Console.WriteLine(code + " completed");
            //Возвращение пути к файлам
            string[] result = { x, y, z, t };
            return result;
        }

        internal static partial string[] NewVoxImage(string[] F, string[] G, int size, double area, string code, bool color)
        {
            Bitmap Bitmap_x = new(size, size, pixelformat);
            Bitmap Bitmap_y = new(size, size, pixelformat);
            Bitmap Bitmap_z = new(size, size, pixelformat);
            Bitmap Bitmap_t = new(size, size, pixelformat);
            LockBitmap lockBitmap_x = new(Bitmap_x);
            LockBitmap lockBitmap_y = new(Bitmap_y);
            LockBitmap lockBitmap_z = new(Bitmap_z);
            LockBitmap lockBitmap_t = new(Bitmap_t);

            Bitmap CxF = new(F[0], false);
            Bitmap CyF = new(F[1], false);
            Bitmap CzF = new(F[2], false);
            Bitmap CtF = new(F[3], false);
            LockBitmap lockCxF = new(CxF);
            LockBitmap lockCyF = new(CyF);
            LockBitmap lockCzF = new(CzF);
            LockBitmap lockCtF = new(CtF);

            Bitmap CxG = new(G[0], false);
            Bitmap CyG = new(G[1], false);
            Bitmap CzG = new(G[2], false);
            Bitmap CtG = new(G[3], false);
            LockBitmap lockCxG = new(CxG);
            LockBitmap lockCyG = new(CyG);
            LockBitmap lockCzG = new(CzG);
            LockBitmap lockCtG = new(CtG);

            double P;
            if (color)
                P = P_RGB;
            else
                P = P_Gray;

            double Xmin = -area;
            double Xmax = area;
            double Ymin = -area;
            double Ymax = area;

            double stepX = (Xmax - Xmin) / size;
            double stepY = (Ymax - Ymin) / size;

            lockBitmap_x.LockBits();
            lockBitmap_y.LockBits();
            lockBitmap_z.LockBits();
            lockBitmap_t.LockBits();

            lockCxF.LockBits();
            lockCyF.LockBits();
            lockCzF.LockBits();
            lockCtF.LockBits();
            lockCxG.LockBits();
            lockCyG.LockBits();
            lockCzG.LockBits();
            lockCtG.LockBits();

            int cores = Environment.ProcessorCount;
            Task[] tasks = new Task[cores];

            for (int tasknumber = 0; tasknumber < tasks.Length; tasknumber++)
            {
                //Разделение области вычисления на равные части каждому потоку
                int start = (size / cores) * tasknumber;
                int end = (size / cores) * (tasknumber + 1);
                tasks[tasknumber] = Task.Run(() =>
                {
                    for (int i = start; i < end; i++)
                    {
                        double x1 = Xmin + i * stepX;
                        double x2 = Xmin + (i + 1) * stepX;
                        double x3 = Xmin + i * stepX;

                        for (int j = 0; j < size - 1; j++)
                        {
                            double y1 = Ymin + j * stepY;
                            double y2 = Ymin + j * stepY;
                            double y3 = Ymin + (j + 1) * stepY;

                            int m1F = GetValue(i, j, lockCxF);
                            int m2F = GetValue(i, j, lockCyF);
                            int m3F = GetValue(i, j, lockCzF);
                            int m4F = GetValue(i, j, lockCtF);

                            int m1G = GetValue(i, j, lockCxF);
                            int m2G = GetValue(i, j, lockCyF);
                            int m3G = GetValue(i, j, lockCzF);
                            int m4G = GetValue(i, j, lockCtF);

                            double n1F = (2 * m1F - P) / P;
                            double n2F = (2 * m2F - P) / P;
                            double n3F = (2 * m3F - P) / P;
                            double n4F = (2 * m4F - P) / P;

                            double n1G = (2 * m1G - P) / P;
                            double n2G = (2 * m2G - P) / P;
                            double n3G = (2 * m3G - P) / P;
                            double n4G = (2 * m4G - P) / P;

                            double zf1 = -x1 * (n1F / n3F) - y1 * (n2F / n3F) - (n4F / n3F);
                            double zf2 = -x2 * (n1F / n3F) - y2 * (n2F / n3F) - (n4F / n3F);
                            double zf3 = -x3 * (n1F / n3F) - y3 * (n2F / n3F) - (n4F / n3F);

                            double zg1 = -x1 * (n1G / n3G) - y1 * (n2G / n3G) - (n4G / n3G);
                            double zg2 = -x2 * (n1G / n3G) - y2 * (n2G / n3G) - (n4G / n3G);
                            double zg3 = -x3 * (n1G / n3G) - y3 * (n2G / n3G) - (n4G / n3G);

                            double zf = zf1;
                            double zg = zg1;
                            double a1, a2, a3, a4;
                            switch (code)
                            {
                                case "plus":
                                    a1 = n1F * n3G + n1G * n3F;
                                    a2 = n2F * n3G + n2G * n3F;
                                    a3 = n3F * n3G;
                                    a4 = n4F * n3G + n4G * n3F;
                                    break;
                                case "minus":
                                    a1 = n1F * n3G - n1G * n3F;
                                    a2 = n2F * n3G - n2G * n3F;
                                    a3 = n3F * n3G;
                                    a4 = n4F * n3G - n4G * n3F;
                                    break;
                                case "multiply":
                                    a1 = n1F * n3G * zg + n1G * n3F * zf;
                                    a2 = n2F * n3G * zg + n2G * n3F * zf;
                                    a3 = 2 * n3G * n3F;
                                    a4 = n4F * n3G * zg + n4G * n3F * zf;
                                    break;
                                default:
                                    a1 = n1F * n3G * zg + n1G * n3F * zf;
                                    a2 = n2F * n3G * zg + n2G * n3F * zf;
                                    a3 = 2 * n3G * n3F;
                                    a4 = n4F * n3G * zg + n4G * n3F * zf;
                                    break;
                            }

                            double n = Sqrt((a1 * a1) + (a2 * a2) + (a3 * a3) + (a4 * a4));
                            double n1 = a1 / n;
                            double n2 = a2 / n;
                            double n3 = a3 / n;
                            double n4 = a4 / n;

                            double Cx = (n1 + 1) * P / 2;
                            double Cy = (n2 + 1) * P / 2;
                            double Cz = (n3 + 1) * P / 2;
                            double Ct = (n4 + 1) * P / 2;

                            lockBitmap_x.SetPixel(i, j, SetColor(Cx, color));
                            lockBitmap_y.SetPixel(i, j, SetColor(Cy, color));
                            lockBitmap_z.SetPixel(i, j, SetColor(Cz, color));
                            lockBitmap_t.SetPixel(i, j, SetColor(Ct, color));
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
            lockBitmap_x.UnlockBits();
            lockBitmap_y.UnlockBits();
            lockBitmap_z.UnlockBits();
            lockBitmap_t.UnlockBits();

            lockCxF.UnlockBits();
            lockCyF.UnlockBits();
            lockCzF.UnlockBits();
            lockCtF.UnlockBits();
            lockCxG.UnlockBits();
            lockCyG.UnlockBits();
            lockCzG.UnlockBits();
            lockCtG.UnlockBits();

            string x = "1" + code + "Cx." + imageformat;
            string y = "1" + code + "Cy." + imageformat;
            string z = "1" + code + "Cz." + imageformat;
            string t = "1" + code + "Ct." + imageformat;

            Bitmap_x.Save(x, imageformat);
            Bitmap_y.Save(y, imageformat);
            Bitmap_z.Save(z, imageformat);
            Bitmap_t.Save(t, imageformat);

            Console.WriteLine(code + " completed");
            string[] result = { x, y, z, t };
            return result;
        }
    }
}