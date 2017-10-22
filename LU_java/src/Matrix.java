/**
 * Created by Jakub Sevcik on 22.10.2017.
 */
public class Matrix {

    private int rows;
    private int cols;
    private double[][] matrix;
    private double[][] matrixL;
    private double[][] matrixU;



    Matrix(int rows, int cols) {
        this.rows = rows;
        this.cols = cols;
        this.matrix = new double[rows][cols];
        initLUMatrix();
    }

    private void initLUMatrix() {
        assert(this.rows == this.cols);

        this.matrixL = new double[rows][cols];
        this.matrixU = new double[rows][cols];

        for(int i = 0; i < rows; i++) {
            this.matrixU[i][i] = 1;
        }
    }

    public void printMatrix() {
        for ( int i = 0 ; i < rows ; i++ ) {
            System.out.print("|");
            for( int j = 0 ; j < cols ; j++ ) {
                System.out.print(this.matrixU[i][j] + " ");
            }
            System.out.print("|\n");
        }
    }
}
