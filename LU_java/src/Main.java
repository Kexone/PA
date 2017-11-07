import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Main {
    static String totalTime;
    public static void main(String[] args) throws IOException {
        BufferedReader stdin = new BufferedReader(new InputStreamReader(System.in));
        while(true) {
            System.out.print("Select count of threads and matrix rows and verbose (y / n):    [int int string]\n");
            String[] input = stdin.readLine().split(" ");
            if (input.length != 3) {
                System.out.print("Error input, pls select it like '1 1'\n");
                continue;
            }
            run(input);
            System.out.println("LU Factorization took: " + totalTime + "s");
        }
    }

    public static void run(String[] par) {
        LUFactorization lu = new LUFactorization(Integer.parseInt(par[1]), Integer.parseInt(par[0]));
        long start = System.currentTimeMillis();
        lu.calculate();
        long time = System.currentTimeMillis() - start;
        totalTime = String.valueOf(((float) time / 1000.0f));
        if(par[2].equals("y"))
            lu.printResults();
    }
}
