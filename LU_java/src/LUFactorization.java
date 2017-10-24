import java.util.Random;

/**
 * Created by Jakub on 24.10.2017.
 */
public class LUFactorization {

    private Matrix matrix;
    private Matrix matrixL;
    private Matrix matrixLT;
    private Matrix matrixU;
    private Matrix matrixPerm;
    private int cThreads;
    private int rows;

    public LUFactorization(int rows, int countThreads) {
        this.rows = rows;
        this.cThreads = countThreads;
        setMat();
        initLUMatrix();
        initPermutationMatrix();
    }

    private void setMat() {
        this.matrix = new Matrix(rows,rows);
        this.matrix.generateMatrix();
    }

    private void initLUMatrix() {
        assert(this.rows == 0);

        this.matrixL = new Matrix(rows,rows);
        this.matrixU = new Matrix(rows,rows);
    }

    private void initPermutationMatrix() {
        this.matrixPerm = new Matrix(rows,rows);
        this.matrixPerm.generateUnitMatrix();
    }

    private Matrix swapCols(Matrix mat, int firstC, int secondC) {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < rows; j++) {
                if(j == firstC || j == secondC) {
                    float tmp = mat.elem(i, firstC);
                    mat.set(i,firstC,mat.elem(i, secondC));
                    mat.set(i,secondC,tmp);
                    break;
                }
            }
        }
        return mat;
    }

    private Matrix calcLower(Matrix mat, Matrix perm){
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < i; j++) {
                if(i == j) {
                    break;
                }
                float tmp  = mat.elem(i,j);
                if(tmp == 0) {
                    continue;
                }
                for(int c = i-1; c >= 0; c--) {
                    if(mat.row(c)[j] == 0) {
                        continue;
                    }
                    for(int r = 0; r < rows; r++) {

                        tmp = mat.row(i)[r] * mat.row(c)[r] - mat.row(i)[r] * mat.row(c)[r];
                        float tmpP ;//= perm.elem(i,j);
                        tmpP = perm.row(i)[r] * perm.row(c)[r] - perm.row(i)[r] * perm.row(c)[r];
                        mat.set(i,j,tmp);
                        perm.set(i,j,tmpP);
                    }
                }

            }
        }
        return mat;
    }

    private float[][] calcUpper(float[][] matA, Matrix unit){
 //       double[][] matA;
   //     matA = mat.getMat();
        for (int k = 0; k < rows - 1; k++) {
            // Get upper val
            float multipliers[] = new float[rows + k + 1];
            float upperVal = (matA[k][k] == 0) ? 1 : matA[k][k];

            // Calculate multipliers for each row
            for (int i = k + 1; i <rows; i++) {
                multipliers[i] = -(matA[i][k] / upperVal);
            }

            // Calc mat
            for (int y = k + 1; y < rows; y++) {
                for (int x = 0; x <  rows; x++) {
                    matA[y][x] = Math.round(matA[y][x] + (multipliers[y] * matA[k][x]));
                    unit.set(y,x, unit.elem(y, x) + (multipliers[y] * unit.elem(k, x)));
                }
            }
        }
        if(matrixLT == null) {
            matrixLT = new Matrix(rows,rows);
            this.matrixLT = unit;

        }
        return matA;
    }


    public void calculate() {
        for (int i = 0; i < rows; i++) {
            if(matrix.row(i)[i] == 0) {
                for (int j = 0; j < rows; j++) {
                    if (matrix.row(i)[j] != 0) {
                        matrix = swapCols(matrix, i, j);
                        matrixPerm = swapCols(matrixPerm, i, j);
                    }
                }
            }
        }

        float[][] origMat = copy(matrix.getMat().clone());
        Matrix unitMat = new Matrix(rows,rows);
        unitMat.generateUnitMatrix();
        this.matrixU.setMat(calcUpper(origMat, unitMat));
        this.matrixL.setMat(calcUpper(matrixLT.getMat(),unitMat));
    }


    public void printResults() {
        System.out.print("__________ \nPermutation Matrix\n");
        this.matrixPerm.printMatrix();
        System.out.print("__________ \nLower Matrix\n");
        this.matrixL.printMatrix();
        System.out.print("__________ \nUper Matrix\n");
        this.matrixU.printMatrix();
        System.out.print("__________ \nGenerated Matrix\n");
        matrix.printMatrix();
    }

    private float[][] copy(float[][] mat) {
        float[][] tmp = new float[rows][rows];
        for (int y = 0; y < rows; y++) {
            for (int x = 0; x <  rows; x++) {
                tmp[y][x] = mat[y][x];
            }
        }
        return tmp;
    }

}
