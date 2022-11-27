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

                            int m1 = GetValue(i, j, lockCxF);
                            int m2 = GetValue(i, j, lockCyF);
                            int m3 = GetValue(i, j, lockCzF);
                            int m4 = GetValue(i, j, lockCtF);

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

                            double n = Sqrt((a1 * a1) + (a2 * a2) + (a3 * a3) + (a4 * a4));
                            n1 = a1 / n;
                            n2 = a2 / n;
                            n3 = a3 / n;
                            n4 = a4 / n;

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