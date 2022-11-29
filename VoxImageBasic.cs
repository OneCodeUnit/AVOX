using System.Drawing.Imaging;

namespace AVOX2
{
    //Частичный класс VoxImageBasic используется для соотвествия программы паттерну "Фасад" 
    internal static partial class VoxImageBasic
    {
        //Палитра и формат изображения
        private const PixelFormat pixelformat = PixelFormat.Format24bppRgb;
        private static readonly ImageFormat imageformat = ImageFormat.Png;
        private const double P_RGB = 16777215;
        private const double P_Gray = 255;

        //Создание М-образов функции
        internal static partial string[] NewImage(int size, double area, string code = "Z", bool color = true);

        //Создание М-образов и рассчёт унарных арифметических функций
        internal static partial string[] NewVoxImage(string[] F, int size, double area, string code, double D, bool color = true);

        //Создание М-образов и рассчёт бинарных арифметических функций
        internal static partial string[] NewVoxImage(string[] F, string[] G, int size, double area, string code, bool color = true);

        //Создание М-образов и рассчёт для логарифмирования
        internal static partial string[] NewVoxImageLogarithm(string[] F, int size, double area, double lbase, double step, bool color = true);
    }
}
