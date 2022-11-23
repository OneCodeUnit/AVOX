using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AVOX2.Calc;

namespace AVOX2
{
    //internal static partial class VoxImageBasic
    //{
    //    internal static partial string[] NewVoxImage(string[] F, string code, double D)
    //    {
    //        Bitmap Bitmap_x = new(size, size, pixelformat);
    //        Bitmap Bitmap_y = new(size, size, pixelformat);
    //        Bitmap Bitmap_z = new(size, size, pixelformat);
    //        Bitmap Bitmap_t = new(size, size, pixelformat);

    //        LockBitmap lockBitmap_x = new(Bitmap_x);
    //        LockBitmap lockBitmap_y = new(Bitmap_y);
    //        LockBitmap lockBitmap_z = new(Bitmap_z);
    //        LockBitmap lockBitmap_t = new(Bitmap_t);

    //        string x = "2" + code + "Cx." + imageformat;
    //        string y = "2" + code + "Cy." + imageformat;
    //        string z = "2" + code + "Cz." + imageformat;
    //        string t = "2" + code + "Ct." + imageformat;

    //        double[,] array_x = new double[size, size];
    //        double[,] array_y = new double[size, size];
    //        double[,] array_z = new double[size, size];
    //        double[,] array_t = new double[size, size];

    //        Bitmap CxF = new(F[0], false);
    //        Bitmap CyF = new(F[1], false);
    //        Bitmap CzF = new(F[2], false);
    //        Bitmap CtF = new(F[3], false);

    //        for (int i = 0; i < size - 1; i++)
    //        {
    //            double x1 = Xmin + i * stepX;
    //            double x2 = Xmin + (i + 1) * stepX;
    //            double x3 = Xmin + i * stepX;

    //            for (int j = 0; j < size - 1; j++)
    //            {
    //                double y1 = Ymin + j * stepY;
    //                double y2 = Ymin + j * stepY;
    //                double y3 = Ymin + (j + 1) * stepY;

    //                double NxF = GetValue(i, j, CxF, P_RGB);
    //                double NyF = GetValue(i, j, CyF, P_RGB);
    //                double NzF = GetValue(i, j, CzF, P_RGB);
    //                double NtF = GetValue(i, j, CtF, P_RGB);

    //                double Zf1 = -x1 * (NxF / NzF) - y1 * (NyF / NzF) - (NtF / NzF);
    //                double Zf2 = -x2 * (NxF / NzF) - y2 * (NyF / NzF) - (NtF / NzF);
    //                double Zf3 = -x3 * (NxF / NzF) - y3 * (NyF / NzF) - (NtF / NzF);

    //                double Zf = Zf1;
    //                double[] NF = { NxF, NyF, NzF, NtF };
    //                double[] A = new double[4];
    //                switch (code)
    //                {
    //                    case "pow":
    //                        A[0] = NxF * Pow(Zf, D - 1);
    //                        A[1] = NyF * Pow(Zf, D - 1);
    //                        A[2] = NzF;
    //                        A[3] = NtF * Pow(Zf, D - 1);
    //                        break;
    //                    case "sqrt":
    //                        A[0] = NxF;
    //                        A[1] = NyF;
    //                        A[2] = NzF * Pow(Zf, 1 / D);
    //                        A[3] = NtF;
    //                        break;
    //                    case "number":
    //                        A[0] = D * NxF;
    //                        A[1] = D * NyF;
    //                        A[2] = NzF;
    //                        A[3] = D * NtF;
    //                        break;
    //                    case "abs":
    //                        if (Zf < 0)
    //                        {
    //                            A[0] = -NxF;
    //                            A[1] = -NyF;
    //                            A[2] = NzF;
    //                            A[3] = -NtF;
    //                        }
    //                        else
    //                        {
    //                            A[0] = NxF;
    //                            A[1] = NyF;
    //                            A[2] = NzF;
    //                            A[3] = NtF;
    //                        }
    //                        break;
    //                }

    //                double norm = Sqrt((A[0] * A[0]) + (A[1] * A[1]) + (A[2] * A[2]) + (A[3] * A[3]));
    //                double Nx = A[0] / norm;
    //                double Ny = A[1] / norm;
    //                double Nz = A[2] / norm;
    //                double Nt = A[3] / norm;

    //                if (Nx == 0) Nx = 0.000000000000000000001;
    //                if (Ny == 0) Ny = 0.000000000000000000001;
    //                if (Nz == 0) Nz = 0.000000000000000000001;
    //                if (Nt == 0) Nt = 0.000000000000000000001;

    //                double Cx = (Nx + 1) * P_RGB / 2;
    //                double Cy = (Ny + 1) * P_RGB / 2;
    //                double Cz = (Nz + 1) * P_RGB / 2;
    //                double Ct = (Nt + 1) * P_RGB / 2;

    //                array_x[i, j] = Cx;
    //                array_y[i, j] = Cy;
    //                array_z[i, j] = Cz;
    //                array_t[i, j] = Ct;
    //            }
    //        }
    //        Thread TX = new(Thread_x);
    //        Thread TY = new(Thread_y);
    //        Thread TZ = new(Thread_z);
    //        Thread TT = new(Thread_t);

    //        void Thread_x()
    //        {
    //            lockBitmap_x.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_x.SetPixel(i, j, SetColor(array_x[i, j]));
    //                }
    //            }
    //            lockBitmap_x.UnlockBits();
    //        }
    //        void Thread_y()
    //        {
    //            lockBitmap_y.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_y.SetPixel(i, j, SetColor(array_y[i, j]));
    //                }
    //            }
    //            lockBitmap_y.UnlockBits();
    //        }
    //        void Thread_z()
    //        {
    //            lockBitmap_z.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_z.SetPixel(i, j, SetColor(array_z[i, j]));
    //                }
    //            }
    //            lockBitmap_z.UnlockBits();
    //        }
    //        void Thread_t()
    //        {
    //            lockBitmap_t.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_t.SetPixel(i, j, SetColor(array_t[i, j]));
    //                }
    //            }
    //            lockBitmap_t.UnlockBits();
    //        }

    //        //Запуск и остановка потоков
    //        TX.Start();
    //        TY.Start();
    //        TZ.Start();
    //        TT.Start();

    //        TX.Join();
    //        TY.Join();
    //        TZ.Join();
    //        TT.Join();
    //        //Сохранение изображений в выбранном формате
    //        NewForceSave(x, Bitmap_x, imageformat);
    //        NewForceSave(y, Bitmap_y, imageformat);
    //        NewForceSave(z, Bitmap_z, imageformat);
    //        NewForceSave(t, Bitmap_t, imageformat);
    //        //Вывод названий файлов в качестве передаваемых значений
    //        string[] relust = { x, y, z, t };
    //        Console.WriteLine("New " + code + " Done");
    //        return relust;
    //    }

    //    internal static partial string[] NewVoxImageGray(string[] F, string code, double D)
    //    {
    //        Bitmap Bitmap_x = new(size, size, grayformat);
    //        Bitmap Bitmap_y = new(size, size, grayformat);
    //        Bitmap Bitmap_z = new(size, size, grayformat);
    //        Bitmap Bitmap_t = new(size, size, grayformat);

    //        LockBitmap lockBitmap_x = new(Bitmap_x);
    //        LockBitmap lockBitmap_y = new(Bitmap_y);
    //        LockBitmap lockBitmap_z = new(Bitmap_z);
    //        LockBitmap lockBitmap_t = new(Bitmap_t);

    //        string x = "3" + code + "Cx." + imageformat;
    //        string y = "3" + code + "Cy." + imageformat;
    //        string z = "3" + code + "Cz." + imageformat;
    //        string t = "3" + code + "Ct." + imageformat;

    //        double[,] array_x = new double[size, size];
    //        double[,] array_y = new double[size, size];
    //        double[,] array_z = new double[size, size];
    //        double[,] array_t = new double[size, size];

    //        Bitmap CxF = new(F[0], false);
    //        Bitmap CyF = new(F[1], false);
    //        Bitmap CzF = new(F[2], false);
    //        Bitmap CtF = new(F[3], false);

    //        for (int i = 0; i < size - 1; i++)
    //        {
    //            double x1 = Xmin + i * stepX;
    //            double x2 = Xmin + (i + 1) * stepX;
    //            double x3 = Xmin + i * stepX;

    //            for (int j = 0; j < size - 1; j++)
    //            {
    //                double y1 = Ymin + j * stepY;
    //                double y2 = Ymin + j * stepY;
    //                double y3 = Ymin + (j + 1) * stepY;

    //                double NxF = GetValue(i, j, CxF, P_Gray);
    //                double NyF = GetValue(i, j, CyF, P_Gray);
    //                double NzF = GetValue(i, j, CzF, P_Gray);
    //                double NtF = GetValue(i, j, CtF, P_Gray);

    //                double Zf1 = -x1 * (NxF / NzF) - y1 * (NyF / NzF) - (NtF / NzF);
    //                double Zf2 = -x2 * (NxF / NzF) - y2 * (NyF / NzF) - (NtF / NzF);
    //                double Zf3 = -x3 * (NxF / NzF) - y3 * (NyF / NzF) - (NtF / NzF);

    //                double Zf = Zf1;
    //                double[] NF = { NxF, NyF, NzF, NtF };
    //                double[] A = new double[4];
    //                switch (code)
    //                {
    //                    case "pow":
    //                        A[0] = NxF * Pow(Zf, D - 1);
    //                        A[1] = NyF * Pow(Zf, D - 1);
    //                        A[2] = NzF;
    //                        A[3] = NtF * Pow(Zf, D - 1);
    //                        break;
    //                    case "sqrt":
    //                        A[0] = NxF;
    //                        A[1] = NyF;
    //                        A[2] = NzF * Pow(Zf, 1 / D);
    //                        A[3] = NtF;
    //                        break;
    //                    case "number":
    //                        A[0] = D * NxF;
    //                        A[1] = D * NyF;
    //                        A[2] = NzF;
    //                        A[3] = D * NtF;
    //                        break;
    //                    case "abs":
    //                        if (Zf < 0)
    //                        {
    //                            A[0] = -NxF;
    //                            A[1] = -NyF;
    //                            A[2] = NzF;
    //                            A[3] = -NtF;
    //                        }
    //                        else
    //                        {
    //                            A[0] = NxF;
    //                            A[1] = NyF;
    //                            A[2] = NzF;
    //                            A[3] = NtF;
    //                        }
    //                        break;
    //                }

    //                double norm = Sqrt((A[0] * A[0]) + (A[1] * A[1]) + (A[2] * A[2]) + (A[3] * A[3]));
    //                double Nx = A[0] / norm;
    //                double Ny = A[1] / norm;
    //                double Nz = A[2] / norm;
    //                double Nt = A[3] / norm;

    //                if (Nx == 0) Nx = 0.000000000000000000001;
    //                if (Ny == 0) Ny = 0.000000000000000000001;
    //                if (Nz == 0) Nz = 0.000000000000000000001;
    //                if (Nt == 0) Nt = 0.000000000000000000001;

    //                double Cx = (Nx + 1) * P_Gray / 2;
    //                double Cy = (Ny + 1) * P_Gray / 2;
    //                double Cz = (Nz + 1) * P_Gray / 2;
    //                double Ct = (Nt + 1) * P_Gray / 2;

    //                array_x[i, j] = Cx;
    //                array_y[i, j] = Cy;
    //                array_z[i, j] = Cz;
    //                array_t[i, j] = Ct;
    //            }
    //        }
    //        Thread TX = new(Thread_x);
    //        Thread TY = new(Thread_y);
    //        Thread TZ = new(Thread_z);
    //        Thread TT = new(Thread_t);

    //        void Thread_x()
    //        {
    //            lockBitmap_x.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_x.SetPixel(i, j, SetColorGray(array_x[i, j]));
    //                }
    //            }
    //            lockBitmap_x.UnlockBits();
    //        }
    //        void Thread_y()
    //        {
    //            lockBitmap_y.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_y.SetPixel(i, j, SetColorGray(array_y[i, j]));
    //                }
    //            }
    //            lockBitmap_y.UnlockBits();
    //        }
    //        void Thread_z()
    //        {
    //            lockBitmap_z.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_z.SetPixel(i, j, SetColorGray(array_z[i, j]));
    //                }
    //            }
    //            lockBitmap_z.UnlockBits();
    //        }
    //        void Thread_t()
    //        {
    //            lockBitmap_t.LockBits();
    //            for (int i = 0; i < size - 1; i++)
    //            {
    //                for (int j = 0; j < size - 1; j++)
    //                {
    //                    lockBitmap_t.SetPixel(i, j, SetColorGray(array_t[i, j]));
    //                }
    //            }
    //            lockBitmap_t.UnlockBits();
    //        }

    //        //Запуск и остановка потоков
    //        TX.Start();
    //        TY.Start();
    //        TZ.Start();
    //        TT.Start();

    //        TX.Join();
    //        TY.Join();
    //        TZ.Join();
    //        TT.Join();
    //        //Сохранение изображений в выбранном формате
    //        NewForceSave(x, Bitmap_x, imageformat);
    //        NewForceSave(y, Bitmap_y, imageformat);
    //        NewForceSave(z, Bitmap_z, imageformat);
    //        NewForceSave(t, Bitmap_t, imageformat);
    //        //Вывод названий файлов в качестве передаваемых значений
    //        string[] relust = { x, y, z, t };
    //        Console.WriteLine("New " + code + " Done");
    //        return relust;
    //    }
    //}
}