using System.Drawing.Imaging;

namespace AVOX2
{
    internal static partial class VoxImageBasic
    {
        //Палитра и формат изображения
        private const PixelFormat pixelformat = PixelFormat.Format24bppRgb;
        private static readonly ImageFormat imageformat = ImageFormat.Png;
        private const double P_RGB = 16777215;
        private const double P_Gray = 255;

        //Создание М-образов функции
        internal static partial string[] NewImage(int size, double area, string code = "Z", bool color = true);

        //Создание М-образов унарных арифметических функций
        internal static partial string[] NewVoxImage(string[] F, int size, double area, string code, double D, bool color = true);
    }
}
