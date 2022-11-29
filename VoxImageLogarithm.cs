using System;
using System.Drawing;
using System.Threading.Tasks;
using static System.Math;
using static AVOX2.Calc;

namespace AVOX2
{
    internal static partial class VoxImageBasic
    {
        internal static partial string[] NewVoxImageLogarithm(string[] F, int size, double area, double lbase, double step, bool color)
        {
            Bitmap Bitmap_x = new(size, size);
            Bitmap Bitmap_y = new(size, size);
            Bitmap Bitmap_z = new(size, size);
            Bitmap Bitmap_t = new(size, size);

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

            for (int i = 0; i < size - 1; i++)
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

                    double ldegree_start, ldegree_end;
                    int k = 1;
                    double D = 0;
                    double start, end;

                    if (zf > 0)
                    {
                        ldegree_start = 0;
                        ldegree_end = 1;
                        //Этап 1. Опеределение диапазона из двух целых степеней основания
                        while (true)
                        {
                            start = Pow(lbase, ldegree_start);
                            end = Pow(lbase, ldegree_end);
                            if ((zf >= start) && (zf <= end))
                            {
                                break;
                            }
                            else
                            {
                                ldegree_start = ldegree_end;
                                ldegree_end = ldegree_end + 1;
                                k++;
                            }
                        }
                        //Итог: целые числа ldegree_start и ldegree_end, между которыми лежит искомая степень
                        Console.WriteLine("Log between " + ldegree_start + " and " + ldegree_end);

                    }
                    //Этап 2. Поиск значения с заданой точнотью между двумя соседними целыми числами (дробные степени)
                    while (true)
                    {
                        start = Pow(lbase, h_start);
                        end = Pow(lbase, h_end);
                        if ((zf >= start) && (zf <= end))
                        {
                            break;
                        }
                        else
                        {
                            h_start = h_end;
                            h_end = h_end + 1;
                            k++;
                        }
                    }

                    //f.Write(string.Format("{0:f1}", D) + " ");
                    //f1.Write(string.Format("{0:f1}", Zf) + " ");

                    D = (D + 1) * 255 / 2;
                    //Zf = (Zf + 1) * P / 2;

                    if (D > 255)
                        D = 255;
                    if (D < 0)
                        D = 0;
                    //if (Zf < 0)
                    //    Zf = 0;
                    //if (Zf > 255)
                    //    Zf = 255;

                    Bitmap_x.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));
                    Bitmap_y.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));
                    Bitmap_z.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));
                    Bitmap_t.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));

                    //Bitmap_xz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
                    //Bitmap_yz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
                    //Bitmap_zz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
                    //Bitmap_tz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
                }

                //f.WriteLine();
                //f1.WriteLine();
            }

            //Watch1.Stop();
            //long temptime1 = Watch1.ElapsedMilliseconds;
            //Watch1.Reset();
            //Console.WriteLine("Время программы " + temptime1);

            string x = "0xh.bmp";
            string y = "0yh.bmp";
            string z = "0zh.bmp";
            string t = "0th.bmp";
            Bitmap_x.Save(x);
            Bitmap_y.Save(y);
            Bitmap_z.Save(z);
            Bitmap_t.Save(t);

            //x = "0xz.bmp";
            //y = "0yz.bmp";
            //z = "0zz.bmp";
            //t = "0tz.bmp";
            //ForceSave(x, Bitmap_xz);
            //ForceSave(y, Bitmap_yz);
            //ForceSave(z, Bitmap_zz);
            //ForceSave(t, Bitmap_tz);

            string[] relust = { x, y, z, t };
            //f.Close();
            //f1.Close();
            Console.WriteLine("Log Done");
            return relust;
        }
    }
}
