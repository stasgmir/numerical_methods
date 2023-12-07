using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeastSquaresMethod
{
    internal class LeastSquaresMethod
    {
        static void Powers(List<double> vector, int polynomialDegree, List<double> powerSums)
        {
            int vectorSize = vector.Count;
            for (int i = 1; i <= 2 * polynomialDegree; i++)
            {
                double sum = 0;
                for (int j = 0; j < vectorSize; j++)
                {
                    sum += Math.Pow(vector[j], i);
                }
                powerSums.Add(sum);
            }
        }

        static void Sum(List<int> powerSums, int polynomialDegree, List<List<double>> sumMatrix)
        {
            int matrixSize = polynomialDegree + 1;
            for (int row = 0; row < matrixSize; row++)
            {
                for (int col = 1; col < matrixSize; col++)
                {
                    sumMatrix[row][col] = powerSums[row + col - 1];
                }
            }
        }

        static void Praw(List<double> t, List<double> C, int polynomialDegree, double[] prawCoefficients)
        {
            int vectorSize = t.Count;
            for (int row = 0; row < polynomialDegree + 1; row++)
            {
                double sum = 0;
                int k1 = row;
                for (int i = 0; i < vectorSize; i++)
                {
                    sum += C[i] * (int)Math.Pow(t[i], k1);
                }
                prawCoefficients[row] = sum;
            }
        }

        public static void SolveLSM(List<double> t, List<double> C, List<double> polynomialCoefficients, int dataSize, int polynomialDegree)
        {
            List<double> powerSums = new List<double>(2 * polynomialDegree + 1); 
            double[,] sumMatrix = new double[polynomialDegree + 1, polynomialDegree + 1];
            double[] prawCoefficients = new double[polynomialDegree + 1];

            Powers(t, polynomialDegree, powerSums);

            int matrixSize = polynomialDegree + 1;
            for (int row = 0; row < matrixSize; row++)
            {
                for (int col = 0; col < matrixSize; col++)
                {
                    sumMatrix[row, col] = powerSums[row + col]; 
                }
            }
            sumMatrix[0, 0] = dataSize;

            Praw(t, C, polynomialDegree, prawCoefficients);

            double[] solution = GaussMethod.Solve(sumMatrix, prawCoefficients);

            polynomialCoefficients.AddRange(solution);
        }
        public static double ResidualVariance(List<double> t, List<double> C, List<double> coefficients, int polynomialDegree, int dataSize)
        {
            double residualVariance = 1.0 / (dataSize - polynomialDegree - 1.0);
            int vectorSize = t.Count;
            double sum = 0;

            for (int i = 0; i < vectorSize; i++)
            {
                sum += Math.Pow(C[i] - coefficients[0] - coefficients[1] * t[i] - coefficients[2] * Math.Pow(t[i], 2), 2);
            }

            residualVariance *= sum;

            return residualVariance;
        }

       public static double ApproximationError(List<double> t, List<double> C, List<double> coefficients, int dataSize)
        {
            double approximationError = 1.0 / dataSize;
            int vectorSize = t.Count;
            double sum = 0;

            for (int i = 0; i < vectorSize; i++)
            {
                sum += Math.Abs((C[i] - (coefficients[0] + coefficients[1] * t[i] + coefficients[2] * Math.Pow(t[i], 2.0))) / C[i]);
            }

            approximationError *= sum * 100.0;

            return approximationError;
        }

    }
}
