using System;
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

        private float[,] OperOnMat(float[,] matA, float[,] unit)
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
                        matA[y, x] = (float)Math.Round(matA[y, x] + (multipliers[y] * matA[k, x]));
                        unit[y, x] = unit[y, x] + (multipliers[y] * unit[k, x]);
                    }
                }
            }
            if (matrixLT == null)
            {
                matrixLT = new Matrix(rows, rows);
                this.matrixLT.setMat(unit);

            }
            return matA;
        }

        private float[,] OperOnMatPar(float[,] matA, float[,] unit)
        {

            for (int k = 0; k < rows - 1; k++)
            {

                float[] multipliers = new float[rows + k + 1];
                float upperVal = (matA[k, k] == 0) ? 1 : matA[k, k];

                Parallel.For(0, rows, index =>
                {
                    for (int i = index; i < rows; i++)
                    {
                        multipliers[i] = -(matA[i, k] / upperVal);
                    }
                });
                Parallel.For(0, rows, y =>
                {
                    for (int x = 0; x < rows; x++)
                    {
                        matA[y, x] = (float) Math.Round(matA[y, x] + (multipliers[y]*matA[k, x]));
                        unit[y, x] = unit[y, x] + (multipliers[y]*unit[k, x]);
                    }
                });
            }
            if (matrixLT == null)
            {
                matrixLT = new Matrix(rows, rows);
                this.matrixLT.setMat(unit);

            }
            return matA;
        }

        public void calculate(bool parallel = false)
        {
            for (int i = 0; i < rows; i++)
            {
                if (matrix.elem(i,i) == 0)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (matrix.elem(i,j) != 0)
                        {
                            matrix = swapCols(matrix, i, j);
                            matrixPerm = swapCols(matrixPerm, i, j);
                        }
                    }
                }
            }
            float[,] origMat = (float[,])this.matrix.getMat().Clone(); ;
            Matrix unitMat = new Matrix(rows, rows);
            unitMat.generateUnitMatrix();
            if (parallel)
            {
                this.matrixU.setMat(OperOnMatPar(origMat, unitMat.getMat()));
                unitMat.generateUnitMatrix(); ;
                this.matrixL.setMat(OperOnMatPar(matrixLT.getMat(), unitMat.getMat()));
            }
            else
            {
                this.matrixU.setMat(OperOnMat(origMat, unitMat.getMat()));
                unitMat.generateUnitMatrix(); ;
                this.matrixL.setMat(OperOnMat(matrixLT.getMat(), unitMat.getMat()));
            }
            
            
            


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
    }
}
