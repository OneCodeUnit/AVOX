using static AVOX2.Arithmetic;
using static AVOX2.Calc;
using static System.Math;
using System;
using System.Drawing;

namespace AVOX2
{
    //Старый класс с вычислениями
    internal static class Operation
    {
        private const System.Drawing.Imaging.PixelFormat pixelformat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
        //static Stopwatch Watch1 = new();
        private const double P = 16777215;
        //private const double P = 255;
        private const int size = VoxImageBasic.size;

        private const double Xmin = VoxImageBasic.Xmin;
        private const double Xmax = VoxImageBasic.Xmax;
        private const double Ymin = VoxImageBasic.Ymin;
        private const double Ymax = VoxImageBasic.Ymax;
        private const double stepX = (Xmax - Xmin) / size;
        private const double stepY = (Ymax - Ymin) / size;

        internal static string[] Image(string code)
        {
            Bitmap Bitmap_x = new(size, size, pixelformat);
            Bitmap Bitmap_y = new(size, size, pixelformat);
            Bitmap Bitmap_z = new(size, size, pixelformat);
            Bitmap Bitmap_t = new(size, size, pixelformat);

            //Watch1.Start();
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

                    double z1, z2, z3;

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
                            //Watch2.Start();
                            z1 = GetZ(x1, y1);
                            z2 = GetZ(x2, y2);
                            z3 = GetZ(x3, y3);
                            //Watch2.Stop();
                            break;
                    }

                    double A1 = y1 * (z2 - z3) - y2 * (z1 - z3) + y3 * (z1 - z2);
                    double A2 = -(x1 * (z2 - z3) - x2 * (z1 - z3) + x3 * (z1 - z2));
                    double A3 = x1 * (y2 - y3) - x2 * (y1 - y3) + x3 * (y1 - y2);
                    double A4 = -(x1 * (y2 * z3 - y3 * z2) - x2 * (y1 * z3 - y3 * z1) + x3 * (y1 * z2 - y2 * z1));
                    //Watch2.Start();
                    double norm = Sqrt((A1 * A1) + (A2 * A2) + (A3 * A3) + (A4 * A4));
                    //Watch2.Stop();
                    double Nx = A1 / norm;
                    double Ny = A2 / norm;
                    double Nz = A3 / norm;
                    double Nt = A4 / norm;

                    if (Nx == 0) Nx = 0.000000000000000000001;
                    if (Ny == 0) Ny = 0.000000000000000000001;
                    if (Nz == 0) Nz = 0.000000000000000000001;
                    if (Nt == 0) Nt = 0.000000000000000000001;

                    double Cx = (Nx + 1) * P / 2;
                    double Cy = (Ny + 1) * P / 2;
                    double Cz = (Nz + 1) * P / 2;
                    double Ct = (Nt + 1) * P / 2;

                    //Bitmap_x.SetPixel(i, j, Color.FromArgb((int)Cx, (int)Cx, (int)Cx));
                    //Bitmap_y.SetPixel(i, j, Color.FromArgb((int)Cy, (int)Cy, (int)Cy));
                    //Bitmap_z.SetPixel(i, j, Color.FromArgb((int)Cz, (int)Cz, (int)Cz));
                    //Bitmap_t.SetPixel(i, j, Color.FromArgb((int)Ct, (int)Ct, (int)Ct));

                    Bitmap_x.SetPixel(i, j, SetColor(Cx, true));
                    Bitmap_y.SetPixel(i, j, SetColor(Cy, true));
                    Bitmap_z.SetPixel(i, j, SetColor(Cz, true));
                    Bitmap_t.SetPixel(i, j, SetColor(Ct, true));
                }
            }
            //Console.WriteLine("ansver " + GetZ(100, 100));
            //Console.WriteLine("Значения " + z1 + " " + z2 + " " + z3);
            //Watch1.Stop();
            //temptime1 = Watch1.ElapsedMilliseconds;
            //temptime2 = Watch2.ElapsedMilliseconds;
            //Watch1.Reset();
            //Watch2.Reset();

            string x = code + "Cx.bmp";
            string y = code + "Cy.bmp";
            string z = code + "Cz.bmp";
            string t = code + "Ct.bmp";
            ForceSave(x, Bitmap_x);
            ForceSave(y, Bitmap_y);
            ForceSave(z, Bitmap_z);
            ForceSave(t, Bitmap_t);
            string[] relust = { x, y, z, t };
            //Console.WriteLine("Время программы " + temptime1);
            //Console.WriteLine("Время возведения " + temptime2);
            Console.WriteLine(code + " Done");
            return relust;
        }

        //internal static string[] VoxImage(string[] F, string code, double D)
        //{
        //    Bitmap Bitmap_x = new(size, size, pixelformat);
        //    Bitmap Bitmap_y = new(size, size, pixelformat);
        //    Bitmap Bitmap_z = new(size, size, pixelformat);
        //    Bitmap Bitmap_t = new(size, size, pixelformat);

        //    Bitmap CxF = new(F[0], false);
        //    Bitmap CyF = new(F[1], false);
        //    Bitmap CzF = new(F[2], false);
        //    Bitmap CtF = new(F[3], false);

        //    //Watch1.Start();
        //    for (int i = 0; i < size - 1; i++)
        //    {
        //        double x1 = Xmin + i * stepX;
        //        double x2 = Xmin + (i + 1) * stepX;
        //        double x3 = Xmin + i * stepX;

        //        for (int j = 0; j < size - 1; j++)
        //        {
        //            double y1 = Ymin + j * stepY;
        //            double y2 = Ymin + j * stepY;
        //            double y3 = Ymin + (j + 1) * stepY;

        //            double NxF = GetValue(i, j, CxF, P);
        //            double NyF = GetValue(i, j, CyF, P);
        //            double NzF = GetValue(i, j, CzF, P);
        //            double NtF = GetValue(i, j, CtF, P);

        //            double Zf = -x1 * (NxF / NzF) - y1 * (NyF / NzF) - (NtF / NzF);
        //            double Zf2 = -x2 * (NxF / NzF) - y2 * (NyF / NzF) - (NtF / NzF);
        //            double Zf3 = -x3 * (NxF / NzF) - y3 * (NyF / NzF) - (NtF / NzF);

        //            double[] NF = { NxF, NyF, NzF, NtF };
        //            double[] A;
        //            //Watch2.Start();
        //            switch (code)
        //            {
        //                case "pow":
        //                    A = VPow(NF, Zf, D);
        //                    break;
        //                case "sqrt":
        //                    A = VSqrt(NF, Zf, D);
        //                    break;
        //                case "number":
        //                    A = VNumber(NF, Zf, D);
        //                    break;
        //                case "abs":
        //                    A = VAbs(NF, Zf, D);
        //                    break;
        //                default:
        //                    A = VPow(NF, Zf, D);
        //                    break;
        //            }
        //            double A1 = A[0];
        //            double A2 = A[1];
        //            double A3 = A[2];
        //            double A4 = A[3];

        //            double norm = Sqrt((A1 * A1) + (A2 * A2) + (A3 * A3) + (A4 * A4));
        //            //Watch2.Stop();

        //            double Nx = A1 / norm;
        //            double Ny = A2 / norm;
        //            double Nz = A3 / norm;
        //            double Nt = A4 / norm;

        //            if (Nx == 0) Nx = 0.000000000000000000001;
        //            if (Ny == 0) Ny = 0.000000000000000000001;
        //            if (Nz == 0) Nz = 0.000000000000000000001;
        //            if (Nt == 0) Nt = 0.000000000000000000001;

        //            double Cx = (Nx + 1) * P / 2;
        //            double Cy = (Ny + 1) * P / 2;
        //            double Cz = (Nz + 1) * P / 2;
        //            double Ct = (Nt + 1) * P / 2;

        //            //Bitmap_x.SetPixel(i, j, Color.FromArgb((int)Cx, (int)Cx, (int)Cx));
        //            //Bitmap_y.SetPixel(i, j, Color.FromArgb((int)Cy, (int)Cy, (int)Cy));
        //            //Bitmap_z.SetPixel(i, j, Color.FromArgb((int)Cz, (int)Cz, (int)Cz));
        //            //Bitmap_t.SetPixel(i, j, Color.FromArgb((int)Ct, (int)Ct, (int)Ct));

        //            Bitmap_x.SetPixel(i, j, SetColor(Cx));
        //            Bitmap_y.SetPixel(i, j, SetColor(Cy));
        //            Bitmap_z.SetPixel(i, j, SetColor(Cz));
        //            Bitmap_t.SetPixel(i, j, SetColor(Ct));
        //        }
        //    }
        //    //Watch1.Stop();
        //    //temptime1 = Watch1.ElapsedMilliseconds;
        //    //temptime2 = Watch2.ElapsedMilliseconds;
        //    //Watch1.Reset();
        //    //Watch2.Reset();

        //    string x = code + "1Cx.bmp";
        //    string y = code + "1Cy.bmp";
        //    string z = code + "1Cz.bmp";
        //    string t = code + "1Ct.bmp";
        //    ForceSave(x, Bitmap_x);
        //    ForceSave(y, Bitmap_y);
        //    ForceSave(z, Bitmap_z);
        //    ForceSave(t, Bitmap_t);
        //    string[] relust = { x, y, z, t };
        //    //Console.WriteLine("Время программы " + temptime1);
        //    //Console.WriteLine("Время возведения " + temptime2);
        //    Console.WriteLine(code + " to " + D + " done");
        //    return relust;
        //}

        //internal static string[] VoxImage(string[] F, string[] G, string code)
        //{
        //    Bitmap Bitmap_x = new(size, size, pixelformat);
        //    Bitmap Bitmap_y = new(size, size, pixelformat);
        //    Bitmap Bitmap_z = new(size, size, pixelformat);
        //    Bitmap Bitmap_t = new(size, size, pixelformat);

        //    Bitmap CxF = new(F[0], false);
        //    Bitmap CyF = new(F[1], false);
        //    Bitmap CzF = new(F[2], false);
        //    Bitmap CtF = new(F[3], false);

        //    Bitmap CxG = new(G[0], false);
        //    Bitmap CyG = new(G[1], false);
        //    Bitmap CzG = new(G[2], false);
        //    Bitmap CtG = new(G[3], false);

        //    for (int i = 0; i < size - 1; i++)
        //    {
        //        double x1 = Xmin + i * stepX;
        //        double x2 = Xmin + (i + 1) * stepX;
        //        double x3 = Xmin + i * stepX;

        //        for (int j = 0; j < size - 1; j++)
        //        {
        //            double y1 = Ymin + j * stepY;
        //            double y2 = Ymin + j * stepY;
        //            double y3 = Ymin + (j + 1) * stepY;

        //            double NxF = GetValue(i, j, CxF, P);
        //            double NyF = GetValue(i, j, CyF, P);
        //            double NzF = GetValue(i, j, CzF, P);
        //            double NtF = GetValue(i, j, CtF, P);

        //            double NxG = GetValue(i, j, CxG, P);
        //            double NyG = GetValue(i, j, CyG, P);
        //            double NzG = GetValue(i, j, CzG, P);
        //            double NtG = GetValue(i, j, CtG, P);

        //            double Zf = -x1 * (NxF / NzF) - y1 * (NyF / NzF) - (NtF / NzF);
        //            double Zg = -x1 * (NxG / NzG) - y1 * (NyG / NzG) - (NtG / NzG);

        //            double[] NF = { NxF, NyF, NzF, NtF };
        //            double[] NG = { NxG, NyG, NzG, NtG };
        //            double[] A;

        //            switch (code)
        //            {
        //                case "plus":
        //                    A = VPlus(NF, NG, Zf, Zg);
        //                    break;
        //                case "minus":
        //                    A = VMinus(NF, NG, Zf, Zg);
        //                    break;
        //                case "multiply":
        //                    A = VMultiply(NF, NG, Zf, Zg);
        //                    break;
        //                default:
        //                    A = VPlus(NF, NG, Zf, Zg);
        //                    break;
        //            }
        //            double A1 = A[0];
        //            double A2 = A[1];
        //            double A3 = A[2];
        //            double A4 = A[3];

        //            double norm = Sqrt((A1 * A1) + (A2 * A2) + (A3 * A3) + (A4 * A4));
        //            double Nx = A1 / norm;
        //            double Ny = A2 / norm;
        //            double Nz = A3 / norm;
        //            double Nt = A4 / norm;

        //            double Cx = (Nx + 1) * P / 2;
        //            double Cy = (Ny + 1) * P / 2;
        //            double Cz = (Nz + 1) * P / 2;
        //            double Ct = (Nt + 1) * P / 2;

        //            Bitmap_x.SetPixel(i, j, SetColor(Cx));
        //            Bitmap_y.SetPixel(i, j, SetColor(Cy));
        //            Bitmap_z.SetPixel(i, j, SetColor(Cz));
        //            Bitmap_t.SetPixel(i, j, SetColor(Ct));
        //        }
        //    }

        //    string x = code + "1Cx.bmp";
        //    string y = code + "1Cy.bmp";
        //    string z = code + "1Cz.bmp";
        //    string t = code + "1Ct.bmp";
        //    ForceSave(x, Bitmap_x);
        //    ForceSave(y, Bitmap_y);
        //    ForceSave(z, Bitmap_z);
        //    ForceSave(t, Bitmap_t);
        //    string[] relust = { x, y, z, t };
        //    Console.WriteLine(code + " done");
        //    return relust;
        //}

        //internal static string[] VLog(string[] F, double lbase, double step)
        //{
        //    //if (File.Exists("log.txt"))
        //    //    File.Delete("log.txt");
        //    //if (File.Exists("logz.txt"))
        //    //    File.Delete("logz.txt");
        //    //StreamWriter f = new StreamWriter("log.txt");
        //    //StreamWriter f1 = new StreamWriter("logz.txt");

        //    Bitmap Bitmap_x = new(size, size);
        //    Bitmap Bitmap_y = new(size, size);
        //    Bitmap Bitmap_z = new(size, size);
        //    Bitmap Bitmap_t = new(size, size);

        //    Bitmap Bitmap_xz = new(size, size);
        //    Bitmap Bitmap_yz = new(size, size);
        //    Bitmap Bitmap_zz = new(size, size);
        //    Bitmap Bitmap_tz = new(size, size);

        //    Bitmap CxF = new(F[0]);
        //    Bitmap CyF = new(F[1]);
        //    Bitmap CzF = new(F[2]);
        //    Bitmap CtF = new(F[3]);

        //    //Watch1.Start();

        //    for (int i = 0; i < size - 1; i++)
        //    {
        //        double x1 = Xmin + i * stepX;
        //        double x2 = Xmin + (i + 1) * stepX;
        //        double x3 = Xmin + i * stepX;

        //        for (int j = 0; j < size - 1; j++)
        //        {
        //            double y1 = Ymin + j * stepY;
        //            double y2 = Ymin + j * stepY;
        //            double y3 = Ymin + (j + 1) * stepY;

        //            double NxF = GetValue(i, j, CxF, P);
        //            double NyF = GetValue(i, j, CyF, P);
        //            double NzF = GetValue(i, j, CzF, P);
        //            double NtF = GetValue(i, j, CtF, P);

        //            double Zf = -x1 * (NxF / NzF) - y1 * (NyF / NzF) - (NtF / NzF);
        //            double Zf2 = -x2 * (NxF / NzF) - y2 * (NyF / NzF) - (NtF / NzF);
        //            double Zf3 = -x3 * (NxF / NzF) - y3 * (NyF / NzF) - (NtF / NzF);

        //            double h_start, h_end;
        //            int k = 1;
        //            double D = 0;
        //            double start, end;

        //            if (Zf >= 1)
        //            {
        //                h_start = 0;
        //                h_end = h_start + 1;
        //                while (true)
        //                {
        //                    start = Pow(lbase, h_start);
        //                    end = Pow(lbase, h_end);
        //                    //f.WriteLine("Iteration " + k);
        //                    //f.WriteLine("Try to find " + Zf + " between " + start + " and " + end);
        //                    if ((Zf >= start) && (Zf <= end))
        //                    {
        //                        //f.WriteLine("Done!");
        //                        if (Zf == start)
        //                            D = h_start;
        //                        else if (Zf == end)
        //                            D = h_end;
        //                        else
        //                        {
        //                            double i_h_start = h_start;
        //                            double i_h_end = h_start + step;
        //                            double i_start, i_end;
        //                            int i_k = 1;
        //                            while (i_h_end < h_end)
        //                            {
        //                                i_start = Pow(lbase, i_h_start);
        //                                i_end = Pow(lbase, i_h_end);
        //                                //f.WriteLine("(second) Iteration " + i_k);
        //                                //f.WriteLine("(second) Try to find " + Zf + " between " + i_start + " and " + i_end);
        //                                if ((Zf >= i_start) && (Zf <= i_end))
        //                                {
        //                                    //f.WriteLine("(second) Done!");
        //                                    if (Zf == i_start)
        //                                        D = i_h_start;
        //                                    else if (Zf == i_end)
        //                                        D = i_h_end;
        //                                    else
        //                                        D = (i_h_start + i_h_end) / 2;
        //                                    break;
        //                                }
        //                                i_h_start = i_h_end;
        //                                i_h_end = i_h_end + step;
        //                                i_k++;
        //                            }
        //                        }
        //                        break;
        //                    }
        //                    h_start = h_end;
        //                    h_end = h_end + 1;
        //                    k++;
        //                }
        //            }
        //            else if (Zf > 0)
        //            {
        //                h_start = 0;
        //                h_end = h_start - 1;
        //                while (true)
        //                {
        //                    start = Pow(lbase, h_start);
        //                    end = Pow(lbase, h_end);
        //                    if ((Zf >= end) && (Zf <= start))
        //                    {
        //                        if (Zf == start)
        //                            D = h_start;
        //                        else if (Zf == end)
        //                            D = h_end;
        //                        else
        //                        {
        //                            double i_h_start = h_start;
        //                            double i_h_end = h_start - step;
        //                            double i_start, i_end;
        //                            int i_k = 1;
        //                            while (i_h_end > h_end)
        //                            {
        //                                i_start = Pow(lbase, i_h_start);
        //                                i_end = Pow(lbase, i_h_end);
        //                                if ((Zf >= i_end) && (Zf <= i_start))
        //                                {
        //                                    if (Zf == i_start)
        //                                        D = i_h_start;
        //                                    else if (Zf == i_end)
        //                                        D = i_h_end;
        //                                    else
        //                                        D = (i_h_start + i_h_end) / 2;
        //                                    break;
        //                                }
        //                                i_h_start = i_h_end;
        //                                i_h_end = i_h_end - step;
        //                                i_k++;
        //                            }
        //                        }
        //                        break;
        //                    }
        //                    h_start = h_end;
        //                    h_end = h_end - 1;
        //                    k++;
        //                }
        //            }
        //            else
        //            {
        //                //f.WriteLine("no degree Zf < 0 = " + Zf);
        //                D = 0;
        //                Zf = 0;
        //            }

        //            //f.Write(string.Format("{0:f1}", D) + " ");
        //            //f1.Write(string.Format("{0:f1}", Zf) + " ");

        //            D = (D + 1) * 255 / 2;
        //            //Zf = (Zf + 1) * P / 2;

        //            if (D > 255)
        //                D = 255;
        //            if (D < 0)
        //                D = 0;
        //            //if (Zf < 0)
        //            //    Zf = 0;
        //            //if (Zf > 255)
        //            //    Zf = 255;

        //            Bitmap_x.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));
        //            Bitmap_y.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));
        //            Bitmap_z.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));
        //            Bitmap_t.SetPixel(i, j, Color.FromArgb((int)D, (int)D, (int)D));

        //            //Bitmap_xz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
        //            //Bitmap_yz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
        //            //Bitmap_zz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
        //            //Bitmap_tz.SetPixel(i, j, Color.FromArgb((int)Zf, (int)Zf, (int)Zf));
        //        }

        //        //f.WriteLine();
        //        //f1.WriteLine();
        //    }

        //    //Watch1.Stop();
        //    //long temptime1 = Watch1.ElapsedMilliseconds;
        //    //Watch1.Reset();
        //    //Console.WriteLine("Время программы " + temptime1);

        //    string x = "0xh.bmp";
        //    string y = "0yh.bmp";
        //    string z = "0zh.bmp";
        //    string t = "0th.bmp";
        //    ForceSave(x, Bitmap_x);
        //    ForceSave(y, Bitmap_y);
        //    ForceSave(z, Bitmap_z);
        //    ForceSave(t, Bitmap_t);

        //    //x = "0xz.bmp";
        //    //y = "0yz.bmp";
        //    //z = "0zz.bmp";
        //    //t = "0tz.bmp";
        //    //ForceSave(x, Bitmap_xz);
        //    //ForceSave(y, Bitmap_yz);
        //    //ForceSave(z, Bitmap_zz);
        //    //ForceSave(t, Bitmap_tz);

        //    string[] relust = { x, y, z, t };
        //    //f.Close();
        //    //f1.Close();
        //    Console.WriteLine("Log Done");
        //    return relust;
        //}

        //internal static string[] logtest()
        //{
        //    Bitmap Bitmap_x = new(size, size, pixelformat);
        //    Bitmap Bitmap_y = new(size, size, pixelformat);
        //    Bitmap Bitmap_z = new(size, size, pixelformat);
        //    Bitmap Bitmap_t = new(size, size, pixelformat);

        //    Bitmap CxF = new("0xh.bmp", false);
        //    Bitmap CyF = new("0yh.bmp", false);
        //    Bitmap CzF = new("0zh.bmp", false);
        //    Bitmap CtF = new("0th.bmp", false);

        //    //Bitmap ZxF = new("0xz.bmp", false);
        //    //Bitmap ZyF = new("0yz.bmp", false);
        //    //Bitmap ZzF = new("0zz.bmp", false);
        //    //Bitmap ZtF = new("0tz.bmp", false);

        //    //Watch1.Start();
        //    for (int i = 0; i < size - 1; i++)
        //    {
        //        double x1 = Xmin + i * stepX;
        //        double x2 = Xmin + (i + 1) * stepX;
        //        double x3 = Xmin + i * stepX;

        //        for (int j = 0; j < size - 1; j++)
        //        {
        //            double y1 = Ymin + j * stepY;
        //            double y2 = Ymin + j * stepY;
        //            double y3 = Ymin + (j + 1) * stepY;

        //            double NxF = GetValue(i, j, CxF, 255);
        //            double NyF = GetValue(i, j, CyF, 255);
        //            double NzF = GetValue(i, j, CzF, 255);
        //            double NtF = GetValue(i, j, CtF, 255);

        //            double z1 = -x1 * (NxF / NzF) - y1 * (NyF / NzF) - (NtF / NzF);
        //            double z2 = -x2 * (NxF / NzF) - y2 * (NyF / NzF) - (NtF / NzF);
        //            double z3 = -x3 * (NxF / NzF) - y3 * (NyF / NzF) - (NtF / NzF);

        //            double A1 = y1 * (z2 - z3) - y2 * (z1 - z3) + y3 * (z1 - z2);
        //            double A2 = -(x1 * (z2 - z3) - x2 * (z1 - z3) + x3 * (z1 - z2));
        //            double A3 = x1 * (y2 - y3) - x2 * (y1 - y3) + x3 * (y1 - y2);
        //            double A4 = -(x1 * (y2 * z3 - y3 * z2) - x2 * (y1 * z3 - y3 * z1) + x3 * (y1 * z2 - y2 * z1));

        //            double norm = Sqrt((A1 * A1) + (A2 * A2) + (A3 * A3) + (A4 * A4));

        //            double Nx = A1 / norm;
        //            double Ny = A2 / norm;
        //            double Nz = A3 / norm;
        //            double Nt = A4 / norm;

        //            double Cx = (Nx + 1) * 255 / 2;
        //            double Cy = (Ny + 1) * 255 / 2;
        //            double Cz = (Nz + 1) * 255 / 2;
        //            double Ct = (Nt + 1) * 255 / 2;

        //            Bitmap_x.SetPixel(i, j, Color.FromArgb((int)Cx, (int)Cx, (int)Cx));
        //            Bitmap_y.SetPixel(i, j, Color.FromArgb((int)Cy, (int)Cy, (int)Cy));
        //            Bitmap_z.SetPixel(i, j, Color.FromArgb((int)Cz, (int)Cz, (int)Cz));
        //            Bitmap_t.SetPixel(i, j, Color.FromArgb((int)Ct, (int)Ct, (int)Ct));
        //            //Bitmap_x.SetPixel(i, j, SetColor(Cx));
        //            //Bitmap_y.SetPixel(i, j, SetColor(Cy));
        //            //Bitmap_z.SetPixel(i, j, SetColor(Cz));
        //            //Bitmap_t.SetPixel(i, j, SetColor(Ct));
        //        }
        //    }

        //    string x = "3Cx.bmp";
        //    string y = "3Cy.bmp";
        //    string z = "3Cz.bmp";
        //    string t = "3Ct.bmp";
        //    ForceSave(x, Bitmap_x);
        //    ForceSave(y, Bitmap_y);
        //    ForceSave(z, Bitmap_z);
        //    ForceSave(t, Bitmap_t);
        //    string[] relust = { x, y, z, t };
        //    Console.WriteLine("test done");
        //    return relust;
        //}

    }
}
