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
            double seconds;

            Benchmark time2 = new();
            time2.Start();
            time2.StartRAM();
            NewImage();
            time2.EndRAM();
            time2.End();
            seconds = time2.GetSeconds();
            Console.WriteLine("Megabytes in use " + time2.GetMemory());
            Console.WriteLine("Optimized RGB " + seconds);

            Console.WriteLine("End");
            Console.ReadKey();
            return 0;
        }
    }
}