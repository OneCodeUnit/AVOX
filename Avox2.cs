global using System.Drawing;
global using static System.Math;
using static AVOX2.Operation;
using static AVOX2.VoxImageBasic;

namespace AVOX2
{
    //В данном класе определяется главная функция программы. Через неё всё вызывается
    class Program
    {
        static int Main()
        {
            Console.WriteLine("Start");
            double seconds;

            //Benchmark time1 = new();
            //time1.Start();
            //time1.StartRAM();
            //Image("Z");
            //time1.EndRAM();
            //time1.End();
            //seconds = time1.GetSeconds();
            //Console.WriteLine("Megabytes in use " + time1.GetMemory());
            //Console.WriteLine("Classical " + seconds);

            Benchmark time2 = new();
            time2.Start();
            time2.StartRAM();
            NewImage("Z", true);
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