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
        private float[,] data;



        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.data = new float[rows, cols];
        }


        public void generateMatrix()
        {
            Random gen = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    float range = 0.4f;
                    int factor = (int) Math.Round(gen.NextDouble() * (100.0f - 0.0f) + 0.0f / range);
                    this.data[i,j] = factor * range;
                }
            }
        }

        public void generateUnitMatrix()
        {
            for (int i = 0; i < rows; i++)
            {
                this.data[i,i] = 1;
            }
        }

        public float[,] getMat()
        {
            return (float[,]) data.Clone();
        }

        public void setMat(float[,] mat)
        {
            this.data = mat;
        }

        public void set(int indR, int indC, float value)
        {
            data[indR,indC] = value;
        }

        public float elem(int indR, int indC)
        {
            return data[indR,indC];
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < rows; i++)
            {
                Console.Write("|");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(this.data[i, j] + " ");
                }
                Console.WriteLine("|");
            }
        }
    }
}
