class GAUSS_method
{
    static void FindPivotElement(decimal[,] matrix, int startRow, int rowCount, int columnCount, out int pivotRow, out decimal pivotElement)
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

    static void SwapRows(decimal[,] matrix, decimal[] vector, int row1, int row2)
    {
        int columnCount = matrix.GetLength(1);

        for (int j = 0; j < columnCount; j++)
        {
            decimal temp = matrix[row1, j];
            matrix[row1, j] = matrix[row2, j];
            matrix[row2, j] = temp;
        }

        decimal tempVector = vector[row1];
        vector[row1] = vector[row2];
        vector[row2] = tempVector;
    }

    static void ReduceToTriangle(decimal[,] matrix, decimal[] vector)
    {
        int rowCount = matrix.GetLength(0);
        int columnCount = matrix.GetLength(1);

        for (int i = 0; i < rowCount - 1; i++)
        {
            int pivotRow;
            decimal pivotElement;
            FindPivotElement(matrix, i, rowCount, columnCount, out pivotRow, out pivotElement);

            if (pivotRow != i)
            {
                SwapRows(matrix, vector, i, pivotRow);
            }

            for (int j = i + 1; j < rowCount; j++)
            {
                decimal quotient = matrix[j, i] / matrix[i, i];
                for (int k = i; k < columnCount; k++)
                {
                    matrix[j, k] -= quotient * matrix[i, k];
                }
                vector[j] -= quotient * vector[i];
            }
        }
    }

    static decimal[] BackwardSubstitution(decimal[,] matrix, decimal[] vector)
    {
        int rowCount = matrix.GetLength(0);
        decimal[] result = new decimal[rowCount];

        for (int i = rowCount - 1; i >= 0; i--)
        {
            decimal sum = 0;
            for (int j = i + 1; j < rowCount; j++)
            {
                sum += matrix[i, j] * result[j];
            }
            result[i] = (vector[i] - sum) / matrix[i, i];
        }

        return result;
    }

    public static decimal[] Solve(decimal[,] matrix, decimal[] vector)
    {
        ReduceToTriangle(matrix, vector);
        return BackwardSubstitution(matrix, vector);
    }

    public static void OutputOfSystemSolutions(decimal[] result)
    {
        for (int i = 0; i < result.Length; ++i)
        {
            Console.WriteLine("\nx" + i + "=" + result[i]);
        }
        Console.WriteLine();
    }

   
    public static void InputMatrix(decimal[,] matrix, int rows, int columns)
    {
        Console.WriteLine("Enter the matrix (one row at a time):");
        for (int i = 0; i < rows; i++)
        {
            Console.WriteLine($"Enter elements for row {i + 1}:");
            string[] elements = Console.ReadLine().Split(' ');

            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = decimal.Parse(elements[j]);
            }
            Console.WriteLine();
        }
    }

    public static void InputVector(decimal[] vector, int rows)
    {
        Console.WriteLine("Enter the vector:");
        for (int i = 0; i < rows; i++)
        {
            vector[i] = decimal.Parse(Console.ReadLine());
        }
    }

    public static void OutputSystem(decimal[,] matrix, decimal[] vector)
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

    public static decimal[] CalculateResidualVector(decimal[,] matrix, decimal[] vector, decimal[]
systemSolution)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);
        decimal[] residualVector = new decimal[rows];

        for (int i = 0; i < rows; i++)
        {
            decimal sum = 0;
            for (int j = 0; j < columns; j++)
            {
                sum += matrix[i, j] * systemSolution[j];
            }
            residualVector[i] = (decimal)vector[i] - sum;
        }

        return residualVector;
    }

    public static decimal FindNorm(decimal[] resudialVector)//3 задание 
    {
        decimal max = Math.Abs(resudialVector[0]);
        for (int i = 0; i < resudialVector.Length; i++)
        {
            if (Math.Abs(resudialVector[i]) > max)
                max = Math.Abs(resudialVector[i]);
        }
        return max;
    }

    public static decimal FindError(decimal[,] matrix_clone, decimal[] result) 
    {
        decimal[] vectorAX = new decimal[matrix_clone.GetLength(0)];
        for (int i = 0; i < matrix_clone.GetLength(0); i++)
        {
            vectorAX[i] = 0;
            for (int j = 0; j < matrix_clone.GetLength(1); j++)
            {
                vectorAX[i] += matrix_clone[i, j] * result[j];

            }
            Console.WriteLine();

        }
        Console.WriteLine("new Vector:\n");
        for (int i = 0; i < vectorAX.Length; i++)
        {
            Console.WriteLine(vectorAX[i]);
        }
        decimal[] second_result_vec = Solve(matrix_clone, vectorAX);
        Console.WriteLine("\nSecond system solution ");
        OutputOfSystemSolutions(second_result_vec);

        decimal numerator = Math.Abs(result[0] - second_result_vec[0]);
        decimal denominator = Math.Abs(result[0]);

        for (int i = 0; i < result.Length; i++)
        {

            if (numerator < Math.Abs(result[i] - second_result_vec[i]) && denominator < Math.Abs(result[i]))
            {
                numerator = Math.Abs(result[i] - second_result_vec[i]);
                denominator = Math.Abs(result[i]);
            }
        }
        decimal delta = numerator / denominator;

        return delta;

    }

    public static void OutputResudialVec(decimal[] resudial)
    {
        for (int i = 0; i < resudial.Length; ++i)
        {
            Console.WriteLine("\nx" + i + "=" + resudial[i]);
        }
        Console.WriteLine();
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Enter rows");
        int rows = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter column");
        int columns = int.Parse(Console.ReadLine());

        decimal[,] matrix = new decimal[rows, columns];
        decimal[] vector = new decimal[rows];
        decimal[,] matrix_clone = new decimal[rows, columns];
        decimal[] resudial_vector = new decimal[rows];
        decimal[] result_vector = new decimal[rows];

        InputMatrix(matrix, rows, columns);
        InputVector(vector, rows);
        matrix_clone = (decimal[,])matrix.Clone();

        Console.WriteLine("Output System:");
        OutputSystem(matrix, vector);
        Console.WriteLine("Output of system solutions:");
        result_vector = Solve(matrix, vector);
        OutputOfSystemSolutions(result_vector);

        resudial_vector = CalculateResidualVector(matrix, vector, result_vector);
        Console.WriteLine("\n\nresudial vector:");
        OutputResudialVec(resudial_vector);

        decimal norm = FindNorm(resudial_vector);
        Console.WriteLine("\nNorm=" + norm);

        decimal delta = FindError(matrix_clone, result_vector);
        Console.WriteLine("Error=" + delta);


    }
}