using System;
class Gauss
{
    public static double[] Solve(double[,] matrix, double[] vector)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);
        for (int i = 0; i < rows - 1; i++)
        {
            // Поиск главного элемента по столбцу
            int maxRow = i;
            double maxElement = matrix[i, i];

            for (int k = i + 1; k < rows; k++)
            {
                if (Math.Abs(matrix[k, i]) > Math.Abs(maxElement))
                {
                    maxElement = matrix[k, i];
                    maxRow = k;
                }
            }

            
            if (maxRow != i)
            {
                SwapRows(matrix, vector, i, maxRow);
            }

            for (int j = i + 1; j < rows; j++)
            {
                double quotient = matrix[j, i] / matrix[i, i];
                for (int k = i; k < columns; k++)
                {
                    matrix[j, k] -= quotient * matrix[i, k];
                }
                vector[j] -= quotient * vector[i];
            }
        }

        double[] result= new double[rows];
        for (int i = rows - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < rows; j++)
            {
                sum += matrix[i, j] * result[j];
            }
            result[i] = (vector[i] - sum) / matrix[i, i];
        }

        return result;
    }

    private static void SwapRows(double[,] matrix, double[] vector, int row1, int row2)
    {
        int columns = matrix.GetLength(1);

        for (int i = 0; i < columns; i++)
        {
            double temp = matrix[row1, i];
            matrix[row1, i] = matrix[row2, i];
            matrix[row2, i] = temp;
        }

        double tempVector = vector[row1];
        vector[row1] = vector[row2];
        vector[row2] = tempVector;
    }

    public static void output_result(double[] result)
    {
        for (int i = 0; i < result.Length; ++i)
        {
            Console.WriteLine("\nx" + i + "=" + result[i]);
        }
        Console.WriteLine();
    }

    public static void input_matrix_and_vector(double[,] matrix, double[] vector, int rows, int columns)
    {
        Console.WriteLine("Enter matrix");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = double.Parse(Console.ReadLine());
            }
        }

        Console.WriteLine("Enter vector");
        for (int i = 0; i < rows; i++)
        {
            vector[i] = double.Parse(Console.ReadLine());
        }
    }

    public static void output_matrix(double[,] matrix, double[] vector)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (j < matrix.GetLength(1) - 1)
                {
                    Console.Write(matrix[i, j] + "*x" + j + " + ");
                }
                else
                {
                    Console.WriteLine(matrix[i, j] + "*x" + j + " = " + vector[i]);
                }
            }
            Console.WriteLine();
        }
    }
    public static double[] CalculateResidualVector(double[,] matrix, double[] vector, double[] result)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);
        double[] residualVector = new double[rows];

        for (int i = 0; i < rows; i++)
        {
            double sum = 0;
            for (int j = 0; j < columns; j++)
            {
                sum += matrix[i, j] * result[j];
            }
            residualVector[i] = vector[i] - sum;
        }

        return residualVector;
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter rows");
        int rows = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter column");
        int columns = int.Parse(Console.ReadLine());

        double[,] matrix = new double[rows, columns];
        double[] vector = new double[rows];
        double[] resudial_vector = new double[rows];

        input_matrix_and_vector(matrix, vector, rows, columns);
        output_matrix(matrix, vector);
        output_result(Solve(matrix, vector));

        resudial_vector=CalculateResidualVector(matrix,vector,Solve(matrix,vector));

        Console.WriteLine("\n\nresudial vector");

        output_result(resudial_vector);



     
    }
}


