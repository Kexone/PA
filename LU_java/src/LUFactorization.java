import java.util.Random;
import java.util.concurrent.Executor;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.IntStream;

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
    private ExecutorService es;
    private int start;
    private int end;

    public LUFactorization(int rows, int countThreads) {
        this.rows = rows;
        this.cThreads = countThreads;
        setThreads();
        setMat();
        initLUMatrix();
        initPermutationMatrix();
    }

    private void setMat() {
        this.matrix = new Matrix(rows,rows);
        this.matrix.generateMatrix();
    }

    private void setThreads() {
        es = Executors.newFixedThreadPool(cThreads);
        for (int t = 0; t < cThreads; t++) {
            this.start = t * (rows / cThreads);
            this.end = (t + 1 == cThreads) ? rows : (t + 1) * (rows / cThreads);
        }
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

            System.out.print("prehodil jsem");
            es.submit((()-> {
                for (int i = start; i < end; i++) {
                    for (int j = 0; j < rows; j++) {
                        if (j == firstC || j == secondC) {
                            float tmp = mat.elem(i, firstC);
                            mat.set(i, firstC, mat.elem(i, secondC));
                            mat.set(i, secondC, tmp);
                            break;
                        }
                    }
                }

            }));
        es.shutdown();
        return mat;
    }

    private float[][] calcUpper(float[][] matA, float[][] unit){
        for (int k = 0; k < rows - 1; k++) {
            // Get upper val
            float multipliers[] = new float[rows + k + 1];
            float upperVal = (matA[k][k] == 0) ? 1 : matA[k][k];
            final int nK = k;
            // Calculate multipliers for each row
            System.out.print("prehodil jsem");
            es.submit((()-> {
                for (int i = start; i < end; i++) {
//                    for (int i = k + 1; i < rows; i++) {
                        multipliers[i] = -(matA[i][nK] / upperVal);
                    }
                //}
            }));
            // Calc mat
            for (int y = k + 1; y < rows; y++) {
                for (int x = 0; x <  rows; x++) {
                    matA[y][x] = Math.round(matA[y][x] + (multipliers[y] * matA[k][x]));
                    unit[y][x] = Math.round(unit[y][x] + (multipliers[y] * unit[k][x]));
                }
            }
        }
        if(matrixLT == null) {
            matrixLT = new Matrix(rows,rows);
            this.matrixLT.setMat(unit);

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
        matrix.printMatrix();

        float[][] origMat = copy(matrix.getMat().clone());
        Matrix unitMat = new Matrix(rows,rows);
        unitMat.generateUnitMatrix();
        this.matrixU.setMat(calcUpper(origMat, unitMat.getMat()));
        unitMat.generateUnitMatrix();
        this.matrixL.setMat(calcUpper(matrixLT.getMat(),unitMat.getMat()));
    }


    public void printResults() {
        System.out.print("__________ \nPermutation Matrix\n");
        this.matrixPerm.printMatrix();
        System.out.print("__________ \nLower Matrix\n");
        this.matrixL.printMatrix();
        System.out.print("__________ \nUpper Matrix\n");
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
