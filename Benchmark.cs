using System.Diagnostics;

namespace AVOX2
{
    //В данном класе задаётся способ измерять время выполнения
    public class Benchmark
    {
        private Stopwatch watch = new Stopwatch();
        private long caclmemory1;
        private long caclmemory2;

        public void Start()
        {
            watch.Start();
        }

        public void End()
        { 
            watch.Stop();
        }

        public double GetSeconds() 
        {
            return watch.ElapsedMilliseconds / 60.0;
        }
        public double GetMSeconds()
        {
            return watch.ElapsedMilliseconds;
        }

        public long StartRAM()
        {
            Process proc = Process.GetCurrentProcess();
            proc.Refresh();
            caclmemory1 = proc.WorkingSet64;
            caclmemory1 /= 1024;
            caclmemory1 /= 1024;
            return caclmemory1;
        }

        public long EndRAM()
        {
            Process proc = Process.GetCurrentProcess();
            proc.Refresh();
            caclmemory2 = proc.WorkingSet64;
            caclmemory2 /= 1024;
            caclmemory2 /= 1024;
            return caclmemory2;
        }

        public long GetMemory()
        {
            return caclmemory2 - caclmemory1;
        }
    }
}