using System;
using static AVOX2.Operation;
using static AVOX2.VoxImageBasic;

namespace AVOX2
{
    class Program
    {
        //Главная функция программы
        static int Main(string[] args)
        {
            Console.WriteLine("Start");
            int size = 1200;
            int area = 10;
            bool color = true;

            NewImage(size, area, "Z", color);
            NewVoxImage(NewImage(size, area, "G", color), size, area, "pow", 2, color);

            Console.WriteLine("End");
            Console.ReadKey();
            return 0;
        }
    }
}