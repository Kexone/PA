using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUdecomposition
{
    public class Matrix
    {
        private int rows;
        private int cols;
        private double[,] matrix;
        private double[,] matrixL;
        private double[,] matrixU;



        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.matrix = new double[rows, cols];
            InitLUMatrix();
        }

        private void InitLUMatrix()
        {
            //Assert(this.rows == this.cols);

            this.matrixL = new double[rows, cols];
            this.matrixU = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                this.matrixU[i, i] = 1;
            }
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < rows; i++)
            {
                Console.Write("|");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(this.matrixU[i, j] + " ");
                }
                Console.WriteLine("|");
            }
        }
    }
}
