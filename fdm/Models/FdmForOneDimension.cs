using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace fdm.Models
{
    public class FdmForOneDimension : Fdm
    {
        [Required(ErrorMessage = "Proszę o podanie liczby iteracji")]
        [Range(1, 100, ErrorMessage = "Proszę o podanie liczby całkowitej z zakresu  od 1 do 100")]
        public int iteration { get; set; }

        [Required(ErrorMessage = "Proszę o podanie wartości kroku czasowego")]
        [Range(0, double.MaxValue, ErrorMessage = "Proszę o podanie poprawnej liczby rzeczywistej")]
        public double dt { get; set; }

        [Required(ErrorMessage = "Proszę o podanie wartości początkowej")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Proszę o podanie poprawnej liczby rzeczywistej")]
        public double initialValue { get; set; }

        public FdmForOneDimension()
        {
            this.n = 4;
            this.iteration = 10;
            this.dt = 0.05;
            this.initialValue = 100;
            this.boundaryValueLeft = 0;
            this.boundaryValueRight = 0;
        }
        
        //algorytm MRS
        //na podstawie programu źródłowego
        public double[][] OneDimensionInTime(bool isThomasMethod)
        {
            //deklaracja macierzy wynikowej
            double[][] resultMatrix = new double[iteration][];
            for (int i = 0; i < iteration; i++) resultMatrix[i] = new double[n];

            //deklaracja pozostałych wymaganych zmiennych
            double r = 1.0;
            double[] T = new double[n];
            double[] W = new double[n];

            double[][] P = new double[n][];
            for (int i = 0; i < n; i++) P[i] = new double[n];

            for (int i = 0; i < n; i++)
            {
                T[i] = 0.0;
                W[i] = 0.0;
                for (int j = 0; j < n; j++)
                    P[i][j] = 0.0;
            }

            r = dt / (Math.Pow(1.0 / n, 2));

            for (int j = 0; j < n; j++)
            {
                P[j][j] = 2 * (1 + r);
                T[j] = initialValue;
            }
            for (int j = 0; j < n - 1; j++)
            {
                P[j][j + 1] = -1 * r;
                P[j + 1][j] = -1 * r;
            }


            //Obliczenia
            for (int i = 0; i < iteration; i++)
            {
                //uzupelnianie tablicy
                if (i == 0)
                {
                    W[0] = (boundaryValueLeft + initialValue) / 2 + 2 * (1 - r) * T[0] + r * T[0 + 1];
                    W[n - 1] = r * T[n - 2] + 2 * (1 - r) * T[n - 1] + (boundaryValueRight + initialValue) / 2;
                    for (int j = 1; j < n - 1; j++)
                        W[j] = r * T[j - 1] + 2 * (1 - r) * T[j] + r * T[j + 1];
                }
                else
                {
                    W[0] = 2 * (1 - r) * T[0] + r * T[0 + 1];
                    W[n - 1] = r * T[n - 2] + 2 * (1 - r) * T[n - 1];
                    for (int j = 1; j < n - 1; j++)
                        W[j] = r * T[j - 1] + 2 * (1 - r) * T[j] + r * T[j + 1];
                }

                //tablica tymczasowa na wynik każdego kroku
                double[] resultForThisIteration = new double[n];

                if (isThomasMethod)
                {
                    //jeśli zostal wybrany algorytm Thomasa
                    resultForThisIteration = this.thomasMethod(P, W);
                    T = resultForThisIteration;
                }
                else
                {
                    //jeśli zostal wybrany algorytm Gaussa
                    resultForThisIteration = this.gaussMethod(P, W, T);
                }

                for (int k = 0; k < n; k++)
                    resultMatrix[i][k] = resultForThisIteration[k];
            }

            return resultMatrix;
        }
    }
}