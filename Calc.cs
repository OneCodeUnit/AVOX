using System;
using System.IO;
using System.Drawing;
using static System.Math;

namespace AVOX2
{
    //В данном класе определяются вспомогательные для вычислений функции
    internal class Calc
    {
        //Перекодирование цвета пикселя выбранной точки в значение
        internal static int GetValue(int x, int y, LockBitmap A)
        {
            Color RGB = A.GetPixel(x, y);
            byte R = RGB.R;
            byte G = RGB.G;
            byte B = RGB.B;
            int color = R + 256 * G + 256 * 256 * B;
            return color;
        }

        internal static double GetValue(int x, int y, Bitmap A, double p)
        {
            Color RGB = A.GetPixel(x, y);
            byte R = RGB.R;
            byte G = RGB.G;
            byte B = RGB.B;
            double color = R + 256 * G + 256 * 256 * B;
            return (2 * color - p) / p;
        }

        //Перекодирование значения в точке в цвет
        internal static Color SetColor(double X, bool color)
        {
            if (color)
            {
                int B = (int)Floor(X / (256 * 256));
                int G = (int)Floor((X - (B * 256 * 256)) / 256);
                int R = (int)Floor(X - (G * 256) - (B * 256 * 256));
                return Color.FromArgb(R, G, B);
            }
            else
            {
                int A = (int)X;
                return Color.FromArgb(A, A, A);
            }
        }
        internal static Color SetColor(double X)
        {
            int B = (int)Floor(X / (256 * 256));
            int G = (int)Floor((X - (B * 256 * 256)) / 256);
            int R = (int)Floor(X - (G * 256) - (B * 256 * 256));
            return Color.FromArgb(R, G, B);
        }

        //Значение функции 1, 2 и 3 в точке x,y
        internal static double GetF(double x, double y)
        {
            double temp = (x*x)/625 + (y*y)/400 - 1;
            //double temp = 5 * (y * Sin(PI * x) + x * x * Cos(PI * y));
            return temp;
        }
        internal static double GetG(double x, double y)
        {
            double temp = x * x + y * y - 625;
            //double temp = 4 / x + y;
            //double temp = 5 * (y * Sin(PI * x) + x * x * Cos(PI * y));
            //double temp = 2 * Sin(Sqrt(x * x + y * y) + 2 * Pow(Abs(y), Abs(x)));
            return temp;
        }
        internal static double GetZ(double x, double y)
        {
            double temp = Log(x * x * x + y, 2);
            //double temp = ((x * x) / 625 + (y * y) / 400 - 1) + (x * x + y * y - 625);
            //double temp = (x * x - y)*(x * x - 8 * x + 12 - y);
            //double temp = Pow((x + 50), 2) + Pow((y + 50), 2) + 144;
            //double temp = Pow(2 * Sin(Sqrt(x * x + y * y) + 2 * y), 3);
            return temp;
        }

        //Варианты алгоритма возведения в степень https://habr.com/ru/post/584662/
        internal static double OldApproximatePower(double b, double e)
        {
            long i = BitConverter.DoubleToInt64Bits(b);
            i = (long)(4606853616395542500L + e * (i - 4606853616395542500L));
            return BitConverter.Int64BitsToDouble(i);
        }
        internal static double BinaryPower(double b, UInt64 e)
        {
            double v = 1d;
            while (e != 0)
            {
                if ((e & 1) != 0)
                {
                    v *= b;
                }
                b *= b;
                e >>= 1;
            }
            return v;
        }
        internal static double FastPowerDividing(double b, double e)
        {
            if (b == 1d || e == 0d)
            {
                return 1d;
            }
            var eAbs = Abs(e);
            var el = Ceiling(eAbs);
            var basePart = OldApproximatePower(b, eAbs / el);
            var result = BinaryPower(basePart, (UInt64)el);

            if (e < 0d)
            {
                return 1d / result;
            }
            return result;
        }
        internal static double FastPowerFractional(double b, double e)
        {
            if (b == 1d || e == 0d)
            {
                return 1d;
            }

            double absExp = Abs(e);
            UInt64 eIntPart = (UInt64)absExp;
            double eFractPart = absExp - eIntPart;
            double result = OldApproximatePower(b, eFractPart) * BinaryPower(b, eIntPart);
            if (e < 0d)
            {
                return 1d / result;
            }
            return result;
        }
        internal static double AnotherApproxPower(double a, double b)
        {
            int tmp = (int)(BitConverter.DoubleToInt64Bits(a) >> 32);
            int tmp2 = (int)(b * (tmp - 1072632447) + 1072632447);
            return BitConverter.Int64BitsToDouble(((long)tmp2) << 32);
        }
        internal static double CyclePower(double a, double e)
        {
            double tmp = a;
            for (int i = 1; i < e; i++)
            {
                tmp *= a;
            }
            return tmp;
        }
    }
}
