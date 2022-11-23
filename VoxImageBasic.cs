using System.Drawing.Imaging;

namespace AVOX2
{
    internal static partial class VoxImageBasic
    {
        //Палитра и формат изображения
        private const PixelFormat pixelformat = PixelFormat.Format24bppRgb;
        private static readonly ImageFormat imageformat = ImageFormat.Jpeg;
        private const double P_RGB = 16777215;
        private const double P_Gray = 255;
        //Размер выходного изображения
        internal const int size = 500;
        //Размер области, на которой находятся значения функции
        internal const double Xmin = -100;
        internal const double Xmax = 100;
        internal const double Ymin = -100;
        internal const double Ymax = 100;
        //Сжатие или растяжение области значений до размера выходного изображения
        private const double stepX = (Xmax - Xmin) / size;
        private const double stepY = (Ymax - Ymin) / size;

        //Создание М-образов функции
        internal static partial string[] NewImage(string code = "Z", bool color = true);

        //Создание М-образов унарных арифметических функций в RGB
        //internal static partial string[] NewVoxImage(string[] F, string code, double D);
    }
}
