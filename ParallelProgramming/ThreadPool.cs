using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    public static class ThreadPool
    {
        public static void Run()
        {
            var n = 10_000_000;
            var range = Enumerable.Range(1, n).ToList();
            var lockObject = new object();

            var sw = new Stopwatch();

            //
            sw.Reset();
            sw.Start();
            var sum2 = range.Select(i => 1.0d / i).Sum();
            sw.Stop();
            Console.WriteLine($"sequantial: {sum2}\t\t{sw.ElapsedMilliseconds}ms");

            //
            sw.Reset();
            sw.Start();
            var sum3 = range.AsParallel().Select(i => 1.0d / i).Sum();
            sw.Stop();
            Console.WriteLine($"parallel linq: {sum3}\t\t{sw.ElapsedMilliseconds}ms");

            //
            sw.Reset();
            sw.Start();
            var sum5 = 0.0d;
            Parallel.ForEach(range, i =>
            {
                lock(lockObject)
                {
                    sum5 += 1.0d / i;
                }
            });
            sw.Stop();
            Console.WriteLine($"parallel.for lock: {sum5}\t{sw.ElapsedMilliseconds}ms");

            //
            sw.Reset();
            sw.Start();
            var sum6 = 0.0d;
            Parallel.ForEach(range, i =>
            {
                sum6 += 1.0d / i;
            });
            sw.Stop();
            Console.WriteLine($"parallel.for nolock: {sum6}\t{sw.ElapsedMilliseconds}ms");

            //
            var partitioner = Partitioner.Create(1, n);
            sw.Reset();
            sw.Start();
            var sum1 = 0.0d;
            Parallel.ForEach(partitioner, (Tuple<int, int> partition) =>
            {
                var threadSum = 0.0d;
                for (int i = partition.Item1; i < partition.Item2; i++)
                {
                    threadSum += 1.0d / i;
                }

                lock (lockObject)
                {
                    sum1 += threadSum;
                }
            });
            sw.Stop();
            Console.WriteLine($"partitioner: {sum1}\t\t{sw.ElapsedMilliseconds}ms");

        }
    }
}
