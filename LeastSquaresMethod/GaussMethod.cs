using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeastSquaresMethod
{
    internal class GaussMethod
    {
        static void FindPivotElement(double[,] matrix, int startRow, int rowCount, int columnCount, out int pivotRow, out double pivotElement)
        {
            pivotRow = startRow;
            pivotElement = matrix[startRow, startRow];

            for (int i = startRow + 1; i < rowCount; i++)
            {
                if (Math.Abs(matrix[i, startRow]) > Math.Abs(pivotElement))
                {
                    pivotElement = matrix[i, startRow];
                    pivotRow = i;
                }
            }
        }

        static void SwapRows(double[,] matrix, double[] vector, int row1, int row2)
        {
            int columnCount = matrix.GetLength(1);

            for (int j = 0; j < columnCount; j++)
            {
                double temp = matrix[row1, j];
                matrix[row1, j] = matrix[row2, j];
                matrix[row2, j] = temp;
            }

            double tempVector = vector[row1];
            vector[row1] = vector[row2];
            vector[row2] = tempVector;
        }

        static void ReduceToTriangle(double[,] matrix, double[] vector)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            for (int i = 0; i < rowCount - 1; i++)
            {
                int pivotRow;
                double pivotElement;
                FindPivotElement(matrix, i, rowCount, columnCount, out pivotRow, out pivotElement);

                if (pivotRow != i)
                {
                    SwapRows(matrix, vector, i, pivotRow);
                }

                for (int j = i + 1; j < rowCount; j++)
                {
                    double quotient = matrix[j, i] / matrix[i, i];
                    for (int k = i; k < columnCount; k++)
                    {
                        matrix[j, k] -= quotient * matrix[i, k];
                    }
                    vector[j] -= quotient * vector[i];
                }
            }
        }

        static double[] BackwardSubstitution(double[,] matrix, double[] vector)
        {
            int rowCount = matrix.GetLength(0);
            double[] result = new double[rowCount];

            for (int i = rowCount - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < rowCount; j++)
                {
                    sum += matrix[i, j] * result[j];
                }
                result[i] = (vector[i] - sum) / matrix[i, i];
            }

            return result;
        }

        public static double[] Solve(double[,] matrix, double[] vector)
        {
            ReduceToTriangle(matrix, vector);
            return BackwardSubstitution(matrix, vector);
        }
    }
}
