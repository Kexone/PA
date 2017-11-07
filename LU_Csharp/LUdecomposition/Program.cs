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
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Matrix rows, verbose:    [int int]");
                string[] input = Console.ReadLine().Split(' ');
                if (input.Length != 2)
                {
                    Console.WriteLine("Error input, pls select it like '1 1'");
                    continue;
                }
                Run(input);
                Console.WriteLine("LU Factorization non-parallel: " + totalTime + "s");
                Run(input, true);
                Console.WriteLine("LU Factorization parallel: " + totalTime + "s");
            }
        }


        static void Run(string[] par, bool parallel = false)
        {
            LUFactorization lu = new LUFactorization(Int32.Parse(par[1]), Int32.Parse(par[0]));
            long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lu.calculate(parallel);
            long time = DateTimeOffset.Now.ToUnixTimeMilliseconds()  - start;
            totalTime = time / 1000.0f;
            if(par[1].Equals("1"))
                lu.printResults();
        }
    }
}
