using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace fdm.Models
{
    public class FdmForTwoDimension : Fdm
    {
        [Required(ErrorMessage = "Proszę o podanie wartości brzegowej górnej")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Proszę o podanie poprawnej liczby rzeczywistej")]
        public double boundaryValueTop { get; set; }

        [Required(ErrorMessage = "Proszę o podanie wartości brzegowej dolnej")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Proszę o podanie poprawnej liczby rzeczywistej")]
        public double boundaryValueBottom { get; set; }


        public FdmForTwoDimension()
        {
            this.n = 4;
            this.boundaryValueLeft = 0;
            this.boundaryValueRight = 1;
            this.boundaryValueTop = 1;
            this.boundaryValueBottom = 0;
        }

        public double[][] TwoDimensionInSpace(bool isThomasMethod)
        {
            //BV (Boundary Values) macież wartości brzegowych
            double[][] BV = new double[n + 1][]; 
            for (int i = 0; i <= n; i++) BV[i] = new double[n + 1];

            for (int i = 1; i < n; i++)
            {
                BV[0][i] = boundaryValueLeft;
                BV[i][0] = boundaryValueTop;
                BV[n][i] = boundaryValueRight;
                BV[i][n] = boundaryValueBottom;
            }

            //macierz współczynników (macierz siedmioprzekątniowa na podstwie ksiązki Majchrzak str. 218)
            int matrixSize = (int)Math.Pow((n - 1), 2);
            double[][] A = new double[matrixSize][];
            for (int i = 0; i < matrixSize; i++) A[i] = new double[matrixSize];

            for (int i = 0; i < matrixSize; i++)
            {
                    A[i][i] = -4;
                    if (i + 1 < matrixSize && (i + 1) % (n - 1) != 0)
                    {
                        A[i + 1][i] = 1;
                        A[i][i + 1] = 1;

                    }
                    if (i + 3 < matrixSize)
                    {
                        A[i + 3][i] = 1;
                        A[i][i + 3] = 1;
                    }
            }

            //wektor wyrazów wolnych na podstawie macierzy BV)
            double[] B = new double[matrixSize];
            for (int i = 0, j = 0, k = 0; i < matrixSize; i++, j++)
            {
                if (j >= n - 1)
                {
                    j %= (n - 1);
                    k++;
                }
                B[i] = -BV[j + 1][k] - BV[j + 2][k + 1] - BV[j + 1][k + 2] - BV[j][k + 1];
            }

            //rozwiązanie układu liniowego metodą eliminacji Gaussa
            double[] X = new double[matrixSize];
            X = gaussMethod(A, B, X);

            
            //utworzenie macierzy wynikowej (wartości brzegowe + wartości rozwiązanego układu)
            double[][] resultMatrix = new double[n + 1][];
            resultMatrix = BV;

            for (int i = 0, j = 0, k = 1; i < matrixSize; i++, j++)
            {
                if (j >= n - 1)
                {
                    j %= (n - 1);
                    k++;
                }
                resultMatrix[j + 1][k] = Math.Round(X[i], 2);
            }

            return resultMatrix;
        }
    }
}