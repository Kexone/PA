import java.awt.peer.SystemTrayPeer;
import java.util.Random;

/**
 * Created by Jakub Sevcik on 22.10.2017.
 */
public class Matrix {

    private int rows;
    private int cols;
    private float[][] data;



    Matrix(int rows, int cols) {
        this.rows = rows;
        this.cols = cols;
        this.data = new float[rows][cols];

    }

    public void generateMatrix() {
        Random gen = new Random();
        for (int i = 0; i <rows; i++) {
            for (int j = 0; j < cols; j++) {
                this.data[i][j] = (float) Math.round(gen.nextFloat() * (100.0f - 0.0f) + 1.0f ) ;
            }
        }
    }

    public void generateUnitMatrix() {
        for (int i = 0; i < rows; i++) {
            this.data[i][i] = 1;
        }
    }

    public float[][] getMat() {
        return data.clone();
    }

    public void setMat(float[][] mat) {
        this.data = mat;
    }

    public float[] row(int ind) {
        return data[ind];
    }

    public void set(int indR,int indC, float value) {
        data[indR][indC] = value;
    }

    public float elem(int indR, int indC) {
        return data[indR][indC];
    }

    public void printMatrix() {
        for ( int i = 0 ; i < rows ; i++ ) {
            int len;
            System.out.print("| ");
            for( int j = 0 ; j < rows ; j++ ) {
                len = 5;
                System.out.print(data[i][j]);
                len -= String.valueOf((data[i][j])).length();
                for (int d = 0; d < len; d++ ) {
                    System.out.print(" ");
                }
            }
            System.out.print("|\n");
        }
        System.out.print("\n");
    }

}
