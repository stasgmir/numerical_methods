class NewtonMethod
{
    static double f1(double x1, double x2) { return Math.Cos(0.4 * x2 + Math.Pow(x1, 2)) + Math.Pow(x2, 2) + Math.Pow(x2, 2)-1.6; }
    static double f1dx1(double x1, double x2) { return -2*x1*Math.Sin(0.4*x2+ Math.Pow(x1, 2))+2*x1; }
    static double f1dx2(double x1, double x2) { return 2*x2-0.4*Math.Sin(Math.Pow(x1, 2)+0.4*x2); }
    static double f2(double x1, double x2) { return 1.5 * Math.Pow(x1, 2) - ((Math.Pow(x2, 2)) / 0.36) - 1; }
    static double f2dx1(double x1, double x2) { return 3*x1; }
    static double f2dx2(double x1, double x2) { return -2*x2/0.36; }

    static void FindPivotElement(List<List<double>> matrix, int startRow, int rowCount, int columnCount, out int pivotRow, out double pivotElement)
    {
        pivotRow = startRow;
        pivotElement = matrix[startRow][startRow];

        for (int i = startRow + 1; i < rowCount; i++)
        {
            if (Math.Abs(matrix[i][startRow]) > Math.Abs(pivotElement))
            {
                pivotElement = matrix[i][startRow];
                pivotRow = i;
            }
        }
    }

    static void SwapRows(List<List<double>> matrix, List<double> vector, int row1, int row2)
    {
        int columnCount = matrix[0].Count;

        for (int j = 0; j < columnCount; j++)
        {
            double temp = matrix[row1][j];
            matrix[row1][j] = matrix[row2][j];
            matrix[row2][j] = temp;
        }

        double tempVector = vector[row1];
        vector[row1] = vector[row2];
        vector[row2] = tempVector;
    }

    static void ReduceToTriangle(List<List<double>> matrix, List<double> vector)
    {
        int rowCount = matrix.Count;
        int columnCount = matrix[0].Count;

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
                double quotient = matrix[j][i] / matrix[i][i];
                for (int k = i; k < columnCount; k++)
                {
                    matrix[j][k] -= quotient * matrix[i][k];
                }
                vector[j] -= quotient * vector[i];
            }
        }
    }

    static List<double> BackwardSubstitution(List<List<double>> matrix, List<double> vector)
    {
        int rowCount = matrix.Count;
        /* List<double> result = new List<double>(rowCount);*/
        List<double> result = new List<double>(new double[rowCount]);

        for (int i = rowCount - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < rowCount; j++)
            {
                sum += matrix[i][j] * result[j];
            }
            result[i] = (vector[i] - sum) / matrix[i][i];
        }

        return result;
    }

    static List<double> SolveGaussMethod(List<List<double>> matrix, List<double> vector)
    {
        ReduceToTriangle(matrix, vector);
        return BackwardSubstitution(matrix, vector);
    }
    static List<List<double>> AnalyticalMethodForJacobiMatrix(double x1, double x2)
    {
        return new List<List<double>> {
            new List<double> { f1dx1(x1, x2), f1dx2(x1, x2) },
            new List<double> { f2dx1(x1, x2), f2dx2(x1, x2) }
        };
    }
    static List<List<double>> CourseDifferenceMethodForJacobiMatrix(double x1, double x2, double relIncrement)
    {
        return new List<List<double>> {
            new List<double> { (f1(x1 + x1 * relIncrement, x2) - f1(x1, x2)) / relIncrement / x1, (f1(x1, x2 + x2 * relIncrement) - f1(x1, x2)) / relIncrement / x2 },
            new List<double> { (f2(x1 + x1 * relIncrement, x2) - f2(x1, x2)) / relIncrement / x1, (f2(x1, x2 + x2 * relIncrement) - f2(x1, x2)) / relIncrement / x2 }
        };
    }
    static void SolveNewtonMethod(double x1, double x2, double firstSolutionError, double secondSolutionError,
         int maxNumberIterations, double relativeIncrement = 0.0)
    {
        double delta1 = Math.Max(Math.Abs(f1(x1, x2)), Math.Abs(f2(x1, x2)));
        double delta2 = 1;
        int iteration = 0;

        if (relativeIncrement != 0)
        {
            Console.WriteLine("Relative increment: " + relativeIncrement + ";\n\n");
        }

        while ((delta1 > firstSolutionError || delta2 > secondSolutionError) && iteration < maxNumberIterations)
        {
            iteration++;
            Console.WriteLine(iteration + ": delta of x1: " + delta1.ToString("F10") + "; delta of x2: " + delta2.ToString("F10") + ";\n");

            List<double> residualVector = new List<double> { -f1(x1, x2), -f2(x1, x2) };
            List<List<double>> jacobiMatrix;

            if (relativeIncrement == 0)
            {
                jacobiMatrix = AnalyticalMethodForJacobiMatrix(x1, x2);
            }
            else
            {
                jacobiMatrix = CourseDifferenceMethodForJacobiMatrix(x1, x2, relativeIncrement);
            }

            List<double> solutionVector = SolveGaussMethod(jacobiMatrix, residualVector);
            x1 += solutionVector[0];
            x2 += solutionVector[1];

            delta1 = Math.Abs(residualVector[0]);
            for (int i = 1; i < residualVector.Count; i++)
            {
                if (delta1 < Math.Abs(residualVector[i]))
                {
                    delta1 = Math.Abs(residualVector[i]);
                }
            }

            double max1 = Math.Abs(x1) < 1 ? Math.Abs(solutionVector[0]) : Math.Abs(solutionVector[0] / x1);
            double max2 = Math.Abs(x2) < 1 ? Math.Abs(solutionVector[1]) : Math.Abs(solutionVector[1] / x2);
            delta2 = Math.Max(max1, max2);
        }

        Console.WriteLine("\nnumber of iteration: " + iteration + ". \nfirst x on this iteration: " + x1.ToString("F15") +
            "; \nsecond x on this iteration: " + x2.ToString("F15") + ".\n");
        Console.WriteLine("\n=====================================================\n\n");
    }
    const int maxNumberIterations = 100;
    const double firstSolutionError = 1e-9;
    const double secondSolutionError = 1e-9;

    static void Main(string[] args)
    {
       

        Console.Write("Enter the initial value for x1: ");
        double x1 = double.Parse(Console.ReadLine());

        Console.Write("Enter the initial value for x2: ");
        double x2 = double.Parse(Console.ReadLine());

        Console.WriteLine("Solution error for x1: {0:0.000000000}", firstSolutionError);
        Console.WriteLine("Solution error for x2: {0:0.000000000}", secondSolutionError);
        Console.WriteLine("Max number of iterations: " + maxNumberIterations);
        Console.WriteLine("\n============================\n");

        List<double> vectorOfRelativeIncrement = new List<double> { 0.01, 0.05, 0.1 };

        SolveNewtonMethod(x1, x2, firstSolutionError, secondSolutionError, maxNumberIterations);

        for (int i = 0; i < vectorOfRelativeIncrement.Count; i++)
        {
            SolveNewtonMethod(x1, x2, firstSolutionError, secondSolutionError, maxNumberIterations, vectorOfRelativeIncrement[i]);
        }
    }

}