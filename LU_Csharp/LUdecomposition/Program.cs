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
                if (input[0].Equals("0"))
                    break;

                Run(input);
            }
        }


        static void Run(string[] par)
        {
            LUFactorization lu = new LUFactorization(Int32.Parse(par[0]), Int32.Parse(par[0]));
            long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lu.calculate();
            start = DateTimeOffset.Now.ToUnixTimeMilliseconds() - start;
            Console.WriteLine("LU Factorization non-parallel: " + start / 1000.0f + "s");
           // lu.printResults();
            lu.clearMat();
            
            start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            lu.calculatePar();
            start = DateTimeOffset.Now.ToUnixTimeMilliseconds()  - start;
            Console.WriteLine("LU Factorization parallel: " + start / 1000.0f + "s");

            if (par[1].Equals("1"))
                lu.printResults();
        }
    }
}
