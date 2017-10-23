using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LUdecomposition
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select count of threads and matrix rows:    [int int]");
                string[] input = Console.ReadLine().Split(' ');
                if (input.Length == 2)
                {
                    Console.WriteLine(input[0] + "   " + input[1]);
                }
                else
                {
                    Console.WriteLine("Error input, pls select it like '1 1'");
                    continue;
                }
                Run(input);
            }
        }


        static void Run(string[] par)
        {
            Matrix mat = new Matrix(Int32.Parse(par[0]), Int32.Parse(par[0]));
            mat.PrintMatrix();
        }
    }
}
