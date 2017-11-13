using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;


namespace LUdecomposition
{
    class Program
    {
        static float totalTime;
        private static LUFactorization lu;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Matrix rows, verbose:    [int int]");
                string[] input = Console.ReadLine().Split(' ');
                if (input.Length < 1)
                {
                    Console.WriteLine("Error input, pls select it like '1 1'");
                    continue;
                }
                lu = new LUFactorization(Int32.Parse(input[0]), Int32.Parse(input[0]));
                Run(input);
                Console.WriteLine("LU Factorization non-parallel: " + totalTime + "s");
                Run(input, true);
                Console.WriteLine("LU Factorization parallel: " + totalTime + "s");
            }
        }


        static void Run(string[] par, bool parallel = false)
        {
            
            if (parallel)
            {
                lu.setAgain();
            }
            long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lu.calculate(parallel);
            long time = DateTimeOffset.Now.ToUnixTimeMilliseconds()  - start;
            totalTime = time / 1000.0f;
            if(par[1].Equals("1"))
                lu.printResults();
        }
    }
}
