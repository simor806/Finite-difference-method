using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace fdm.Models
{
    public class Fdm
    {
        [Required(ErrorMessage = "Proszę o podanie liczby elementów siatki")]
        [Range(1, 20, ErrorMessage = "Proszę o podanie liczby całkowitej z zakresu  od 1 do 20")]
        public int n { get; set; }

        [Required(ErrorMessage = "Proszę o podanie wartości brzegowej lewej")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Proszę o podanie poprawnej liczby rzeczywistej")]
        public double boundaryValueLeft { get; set; }

        [Required(ErrorMessage = "Proszę o podanie wartości brzegowej prawej")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Proszę o podanie poprawnej liczby rzeczywistej")]
        public double boundaryValueRight { get; set; }

        public double[][] result;

        protected double[] thomasMethod(double[][] A, double[] B)
        {
            //dane testowe

            //int n = 5;
            //double[] a = new double[5] { 0, 1, 1, 1, 1 };
            //double[] b = new double[5] { 1, 2, 2, 2, 1 };
            //double[] c = new double[5] { 3, 1, 1, 1, 0 };
            //double[] d = new double[5] { 1, 5, 3, 2, 2 };

            // double[][] AB = { new double[] {1,1,0,0,0}, 
            //        new double[] {3,2,1,0,0},    
            //        new double[] {0,1,2,1,0},
            //        new double[] {0,0,1,2,1},
            //        new double[] {0,0,0,1,1}};
            // double[] d = new double[5] { 1, 5, 3, 2, 2 };
            //AA = AB;
            // n = 5;
            // wynik = {2.5, -0.5, 3.5, -3.5, 5.5}

            int n = B.Length;

            //utworzenie wymaganaych tablic           
            double[] a = new double[n];
            double[] b = new double[n];
            double[] c = new double[n];
            double[] d = new double[n];
            double[] beta = new double[n];
            double[] gamma = new double[n];
            double[] x = new double[n];

            //zapełnienie tablic a,b,c i d
            for (int i = 0; i < n; i++)
            {
                b[i] = A[i][i];
                d[i] = B[i];
                if (i != n - 1)
                {
                    a[i + 1] = A[i][i + 1];
                    c[i] = A[i + 1][i];
                }
            }

            //zapełnienie tablic wspłóczynników beta i gamma
            beta[0] = -1 * c[0] / b[0];
            gamma[0] = d[0] / b[0];

            for (int i = 1; i < n; i++)
            {
                beta[i] = -1 * c[i] / (a[i] * beta[i - 1] + b[i]);
                gamma[i] = (d[i] - a[i] * gamma[i - 1]) / (a[i] * beta[i - 1] + b[i]);
            }

            //wyznaczenie wartości niewiadomych
            x[n - 1] = gamma[n - 1];

            for (int i = n - 2; i >= 0; i--)
                x[i] = beta[i] * x[i + 1] + gamma[i];

            //zwrócenie wyniku
            return x;
        }

        //algorytm eliminacji Gaussa
        //na podstawie programu źródłowego
        protected double[] gaussMethod(double[][] A, double[] B, double[] X)
        {
            int n = B.Length;

            double[][] C = new double[n][];
            for (int i = 0; i < n; i++)
                C[i] = new double[n + 1];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    C[i][j] = A[i][j];
                C[i][n] = B[i];
            }

            for (int k = 0; k < n - 1; k++)
                for (int i = k + 1; i < n; i++)
                    for (int j = k + 1; j < n + 1; j++)
                        C[i][j] = C[i][j] - C[k][j] * C[i][k] / C[k][k];

            X[n - 1] = C[n - 1][n] / C[n - 1][n - 1];

            for (int i = n - 2; i >= 0; i--)
            {
                double sum = 0;
                for (int k = i + 1; k < n; k++)
                    sum = sum + C[i][k] * X[k];
                X[i] = (C[i][n] - sum) / C[i][i];
            }

            return X;
        }
    }
}