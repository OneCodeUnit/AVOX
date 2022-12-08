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
            int size = 5000;
            int area = 100;
            bool color = true;
            Benchmark time1 = new Benchmark();
            Benchmark time2 = new Benchmark();

            //string[] A = NewImage(size, area, "F", color);
            //string[] B = NewImage(size, area, "G", color);

            time2.StartRAM();
            time2.Start();
            string[] C = NewImage(size, area, "Z", color);
            time2.End();
            time2.EndRAM();
            Console.WriteLine("Optimize. Time = " + time2.GetSeconds() + ". Memory = " + time2.GetMemory());
            //NewVoxImage(NewImage(size, area, "F", color), NewImage(size, area, "G", color), size, area, "plus", color);

            Console.WriteLine("End");
            Console.ReadKey();
            return 0;
        }
    }
}