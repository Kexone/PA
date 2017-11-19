using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUdecomposition
{
    class LUFactorization
    {
        private Matrix matrix;
        private Matrix matrixL;
        private Matrix matrixLT;
        private Matrix matrixU;
        private Matrix matrixPerm;
        private int cThreads;
        private int rows;

        public LUFactorization(int rows, int countThreads)
        {
            this.rows = rows;
            this.cThreads = countThreads;
            setMat();
            initLUMatrix();
            initPermutationMatrix();
        }

        private void setMat()
        {
            this.matrix = new Matrix(rows, rows);
            this.matrix.generateMatrix();
        }

        public void clearMat()
        {
            matrixLT.clearMatrix();
            matrixL.clearMatrix();
            matrixU.clearMatrix();
            matrixLT = null;
        }

        private void initLUMatrix()
        {
            this.matrixL = new Matrix(rows, rows);
            this.matrixU = new Matrix(rows, rows);
        }

        private void initPermutationMatrix()
        {
            this.matrixPerm = new Matrix(rows, rows);
            this.matrixPerm.generateUnitMatrix();
        }

        private Matrix swapCols(Matrix mat, int firstC, int secondC)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (j == firstC || j == secondC)
                    {
                        float tmp = mat.elem(i, firstC);
                        mat.set(i, firstC, mat.elem(i, secondC));
                        mat.set(i, secondC, tmp);
                        break;
                    }
                }
            }
            return mat;
        }

        private float[,] OperOnMat(float[,] matA, float[,] unit, bool getUnit)
        {

            for (int k = 0; k < rows - 1; k++)
            {

                float[] multipliers = new float[rows + k + 1];
                float upperVal = (matA[k, k] == 0) ? 1 : matA[k, k];

                for (int i = k + 1; i < rows; i++)
                {
                    multipliers[i] = -(matA[i, k] / upperVal);
                }

                for (int y = k + 1; y < rows; y++)
                {
                    for (int x = 0; x < rows; x++)
                    {
                        matA[y, x] = matA[y, x] + (multipliers[y] * matA[k, x]);
                        unit[y, x] = unit[y, x] + (multipliers[y] * unit[k, x]);
                    }
                }
            }
            if (matrixLT == null)
            {
                matrixLT = new Matrix(rows, rows);
                this.matrixLT.setMat(unit);
            }
            if (getUnit)
                return unit;
            return matA;
        }

        private float[,] OperOnMatPar(float[,] matA, float[,] unit, bool getUnit)
        {
            for (int k = 0; k < rows; k++)
            {

                float[] multipliers = new float[rows + k + 1];
                float upperVal = (matA[k, k] == 0) ? 1 : matA[k, k];
                int cK = k;
                Parallel.For(k+1, rows, index =>
                {
                  //  for (int i = 0; i < rows; i++)
                   // {
                        multipliers[index] = -(matA[index, cK] / upperVal);
//                    }

                });
                Parallel.For(k+1, rows, y =>
                {
                    for (int x = 0; x < rows; x++)
                        {
                            matA[y, x] = matA[y, x] + (multipliers[y]*matA[cK, x]);
                            unit[y, x] = unit[y, x] + (multipliers[y]*unit[cK, x]);
                        }
                });
            }
            if (matrixLT == null)
            {
                matrixLT = new Matrix(rows, rows);
                this.matrixLT.setMat(unit);
            }
            if (getUnit)
                return unit;
            return matA;
        }

        public void calculate()
        {
            for (int i = 0; i < rows; i++)
            {
                if (matrix.elem(i, i) == 0)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (matrix.elem(i, j) != 0)
                        {
                            matrix = swapCols(matrix, i, j);
                            matrixPerm = swapCols(matrixPerm, i, j);
                        }
                    }
                }
            }
            float[,] origMat = (float[,])this.matrix.getMat().Clone();
            Matrix unitMat = new Matrix(rows, rows);
            unitMat.generateUnitMatrix();
            
            this.matrixU.setMat(OperOnMat(matrix.getMat(), unitMat.getMat(), false));
            unitMat.generateUnitMatrix();
            this.matrixL.setMat(OperOnMat(matrixLT.getMat(), unitMat.getMat(), true));
        }

        public void calculatePar()
        {
            Parallel.For(0, rows, i =>
            {
                if (matrix.elem(i, i) == 0)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (matrix.elem(i, j) != 0)
                        {
                            matrix = swapCols(matrix, i, j);
                            matrixPerm = swapCols(matrixPerm, i, j);
                        }
                    }
                }
            });
            
            float[,] origMat = (float[,])this.matrix.getMat().Clone();
            Matrix unitMat = new Matrix(rows, rows);
            unitMat.generateUnitMatrix();
            this.matrixU.setMat(OperOnMatPar(origMat, unitMat.getMat(), false));
            unitMat.generateUnitMatrix();
            this.matrixL.setMat(OperOnMatPar(matrixLT.getMat(), unitMat.getMat(), true));
        }


        public void printResults()
        {
            Console.Write("__________ \nPermutation Matrix\n");
            this.matrixPerm.PrintMatrix();
            Console.Write("__________ \nLower Matrix\n");
            this.matrixL.PrintMatrix();
            Console.Write("__________ \nUper Matrix\n");
            this.matrixU.PrintMatrix();
            Console.Write("__________ \nGenerated Matrix\n");
            matrix.PrintMatrix();
        }

        public void test(float[,] data)
        {
            for (int i = 0; i < rows; i++)
            {
                Console.Write("|");
                for (int j = 0; j < rows; j++)
                {
                    Console.Write(data[i, j] + " ");
                }
                Console.WriteLine("|");
            }
        }
    }


   
}
