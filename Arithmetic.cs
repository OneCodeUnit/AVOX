namespace AVOX2
{
    //В данном класе вычисляются значения воксельным подходом
    internal static class Arithmetic
    {
        internal static double[] VPow(double[] NF, double Zf, double D)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double A1 = NxF * Pow(Zf, D - 1);
            double A2 = NyF * Pow(Zf, D - 1);
            double A3 = NzF;
            double A4 = NtF * Pow(Zf, D - 1);

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

        internal static double[] VSqrt(double[] NF, double Zf, double D)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double A1 = NxF;
            double A2 = NyF;
            double A3 = NzF * Pow(Zf, 1 / D);
            double A4 = NtF;

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

        internal static double[] VNumber(double[] NF, double Zf, double D)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double A1 = D * NxF;
            double A2 = D * NyF;
            double A3 = NzF;
            double A4 = D * NtF;

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

        internal static double[] VAbs(double[] NF, double Zf, double D)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double A1, A2, A3, A4;

            if (Zf < 0)
            {
                A1 = -NxF;
                A2 = -NyF;
                A3 = NzF;
                A4 = -NtF;
            }
            else
            {
                A1 = NxF;
                A2 = NyF;
                A3 = NzF;
                A4 = NtF;
            }

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

        internal static double[] VPlus(double[] NF, double[] NG, double Zf, double Zg)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double NxG = NG[0];
            double NyG = NG[1];
            double NzG = NG[2];
            double NtG = NG[3];

            double A1 = NxF * NzG + NxG * NzF;
            double A2 = NyF * NzG + NyG * NzF;
            double A3 = NzF * NzG;
            double A4 = NtF * NzG + NtG * NzF;

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

        internal static double[] VMinus(double[] NF, double[] NG, double Zf, double Zg)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double NxG = NG[0];
            double NyG = NG[1];
            double NzG = NG[2];
            double NtG = NG[3];

            double A1 = NxF * NzG - NxG * NzF;
            double A2 = NyF * NzG - NyG * NzF;
            double A3 = NzF * NzG;
            double A4 = NtF * NzG - NtG * NzF;

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

        internal static double[] VMultiply(double[] NF, double[] NG, double Zf, double Zg)
        {
            double NxF = NF[0];
            double NyF = NF[1];
            double NzF = NF[2];
            double NtF = NF[3];

            double NxG = NG[0];
            double NyG = NG[1];
            double NzG = NG[2];
            double NtG = NG[3];

            double A1 = NxF * NzG * Zg + NxG * NzF * Zf;
            double A2 = NyF * NzG * Zg + NyG * NzF * Zf;
            double A3 = 2 * NzG * NzF;
            double A4 = NtF * NzG * Zg + NtG * NzF * Zf;

            double[] relust = { A1, A2, A3, A4 };
            return relust;
        }

    }
}
