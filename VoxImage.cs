using System;
using System.Drawing;
using System.Threading.Tasks;
using static System.Math;
using static AVOX2.Calc;

namespace AVOX2
{
    internal static partial class VoxImageBasic
    {
        internal static partial string[] NewImage(string code, bool color)
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

            //Выбор палитры
            double P;
            if (color)
                P = P_RGB;
            else
                P = P_Gray;

            //Блокировка
            lockBitmap_x.LockBits();
            lockBitmap_y.LockBits();
            lockBitmap_z.LockBits();
            lockBitmap_t.LockBits();

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

                            double z1, z2, z3;

                            //Вычисления значений функции в точке
                            switch (code)
                            {
                                case "F":
                                    z1 = GetF(x1, y1);
                                    z2 = GetF(x2, y2);
                                    z3 = GetF(x3, y3);
                                    break;
                                case "G":
                                    z1 = GetG(x1, y1);
                                    z2 = GetG(x2, y2);
                                    z3 = GetG(x3, y3);
                                    break;
                                default:
                                    z1 = GetZ(x1, y1);
                                    z2 = GetZ(x2, y2);
                                    z3 = GetZ(x3, y3);
                                    break;
                            }

                            double A1 = y1 * (z2 - z3) - y2 * (z1 - z3) + y3 * (z1 - z2);
                            double A2 = -(x1 * (z2 - z3) - x2 * (z1 - z3) + x3 * (z1 - z2));
                            double A3 = x1 * (y2 - y3) - x2 * (y1 - y3) + x3 * (y1 - y2);
                            double A4 = -(x1 * (y2 * z3 - y3 * z2) - x2 * (y1 * z3 - y3 * z1) + x3 * (y1 * z2 - y2 * z1));

                            double norm = Sqrt((A1 * A1) + (A2 * A2) + (A3 * A3) + (A4 * A4));
                            double Nx = A1 / norm;
                            double Ny = A2 / norm;
                            double Nz = A3 / norm;
                            double Nt = A4 / norm;

                            //if (Nx == 0) Nx = 0.000000000000000000001;
                            //if (Ny == 0) Ny = 0.000000000000000000001;
                            //if (Nz == 0) Nz = 0.000000000000000000001;
                            //if (Nt == 0) Nt = 0.000000000000000000001;

                            double Cx = (Nx + 1) * P / 2;
                            double Cy = (Ny + 1) * P / 2;
                            double Cz = (Nz + 1) * P / 2;
                            double Ct = (Nt + 1) * P / 2;

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

            //Наименования для выходных файлов и возвращаемых значений
            string x = "0" + code + "Cx." + imageformat;
            string y = "0" + code + "Cy." + imageformat;
            string z = "0" + code + "Cz." + imageformat;
            string t = "0" + code + "Ct." + imageformat;
            //Сохранение изображений в выбранном формате
            Bitmap_x.Save(x, imageformat);
            Bitmap_y.Save(y, imageformat);
            Bitmap_z.Save(z, imageformat);
            Bitmap_t.Save(t, imageformat);

            Console.WriteLine("New " + code + " Done");
            //Возвращение пути к файлам
            string[] result = { x, y, z, t };
            return result;
        }
    }
}
