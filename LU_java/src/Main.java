import com.sun.javafx.scene.layout.region.Margins;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) throws IOException {
        while(true) {
            System.out.print("Select count of threads and matrix rows:    [int int]\n");

            BufferedReader stdin = new BufferedReader(new InputStreamReader(System.in));
            String line = stdin.readLine();
            String[] input = line.split(" ");
            if (input.length == 2) {
                System.out.println((input[0] + "   " + input[1]));
            } else {
                System.out.print("Error input, pls select it like '1 1'\n");
                continue;
            }
            run(input);
        }
    }

    public static void run(String[] par) {
        Matrix mat = new Matrix(Integer.parseInt(par[0]), Integer.parseInt(par[0]));
        mat.printMatrix();
    }
}
