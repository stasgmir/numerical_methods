namespace LeastSquaresMethod
{
    internal class SolveLab4
    {
        static void Main()
        {
            List<double> t = new List<double> { 0.0, 5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0,
                40.0, 45.0, 50.0,55.0,60.0,65.0,70.0,75.0,80.0,85.0,90.0,95.0,100.0 };
            List<double> C = new List<double> { 1.00762, 1.00392, 1.00153, 1.00000, 0.99907, 0.99852, 0.99826
                , 0.99818, 0.99828, 0.99849, 0.99878,0.99919,0.99967,1.00024,1.00091,1.00167,1.00253
            ,1.00351,1.00461,1.00586,1.00721};
            List<double> coefficients = new List<double>();

            int N = 22;
            int polynomialDegree = 2;
            double residualVariance = 0;


            LeastSquaresMethod.SolveLSM(t, C, coefficients, N, polynomialDegree);
            residualVariance = LeastSquaresMethod.ResidualVariance(t, C, coefficients, polynomialDegree, N);

            Console.Write("Coefficients: ");
            foreach (var coefficient in coefficients)
            {
                Console.Write(coefficient + " ");
            }

            Console.WriteLine("\nResidual Variance: " + Math.Sqrt(residualVariance));
            Console.WriteLine("\nApproximation Error: " + LeastSquaresMethod.ApproximationError(t, C, coefficients, N));

            Console.WriteLine();

            int vectorSize = t.Count;

            for (int i = 0; i < vectorSize; i++)
            {
                Console.WriteLine("t = " + t[i] + ", C = " + (coefficients[0] + coefficients[1] * t[i] + coefficients[2] * Math.Pow(t[i], 2.0)));
            }
        }
    }
}
