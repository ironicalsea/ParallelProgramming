using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ThreadPool.Run();
            Console.ReadKey();
        }
    }
}