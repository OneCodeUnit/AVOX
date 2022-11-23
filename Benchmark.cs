using System.Diagnostics;

namespace AVOX2
{
    //В данном класе задаётся способ измерять время выполнения
    public class Benchmark
    {
        private Stopwatch watch = new Stopwatch();
        private long startmemory;
        private long endmemory;

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
            startmemory = proc.WorkingSet64;
            startmemory /= 1024;
            startmemory /= 1024;
            return startmemory;
        }

        public long EndRAM()
        {
            Process proc = Process.GetCurrentProcess();
            proc.Refresh();
            endmemory = proc.WorkingSet64;
            endmemory /= 1024;
            endmemory /= 1024;
            return endmemory;
        }

        public long GetMemory()
        {
            return endmemory - startmemory;
        }
    }
}