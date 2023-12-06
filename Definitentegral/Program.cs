using System.Diagnostics.Metrics;

class DefiniteIntegral
{
     static double Func(double x)
    {

        return Math.Sqrt(1 + 2 * Math.Pow(x, 3));

    }
    static double Func2(int i, int j, int n)
    {
        double A = -1, B = 1;
        double C = -1, D = 1;
        double h_x = (B - A) / (2 * n);
        double h_y = (D - C) / (2 * n);
        double x = A + i * h_x, y = C + j * h_y;
        if (x > 1 || y > 1)
            return 0;
        else
            return x * x + 2.0 * y;
    }

   static double TrapezoidMethod(double a, double b, int n, in double E)
    {
        double h = (b - a) / n;
        double sum = Func(a) + Func(b);
        double integral_prev = 0;

        for (int i = 1; i < n - 1; i++)
        {
            sum += 2.0 * Func(a + i * h);
        }

        sum *= h / 2;
        double integral = sum;

        do
        {
            n *= 2;//to improve the accuracy of calculating the integral
            h /= 2;
            double sum_prev = sum;
            sum = Func(a) + Func(b);

            for (int i = 1; i < n - 1; i++)
            {
                sum += 2.0 * Func(a + i * h);
            }

            sum *= h / 2;
            integral_prev = integral;
            integral = sum;

        } while (Math.Abs(integral - integral_prev) > 3 * E); 

        double error = Math.Abs((integral_prev - integral) / (Math.Pow(0.5, 2) - 1));//runge formula

        Console.WriteLine("error: " + error);
        Console.WriteLine("result: " + integral);

        return integral;
    }

    static double SimpsonMethod(double a, double b, int n, in double E)
    {
        double h = (b - a) / n;
        double sum = Func(a) + Func(b);
        double integral_prev = 0;

        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            sum += (i % 2 == 0) ? 2.0 * Func(x) : 4.0 * Func(x);
        }

        double integral = (h / 3) * sum;

        do
        {
            n *= 2;
            h /= 2;
            double sum_prev = sum;
            sum = Func(a) + Func(b);

            for (int i = 1; i < n; i++)
            {
                double x = a + i * h;
                sum += (i % 2 == 0) ? 2.0 * Func(x) : 4.0 * Func(x);
            }

            integral_prev = integral;
            integral = (h / 3) * sum;

        } while (Math.Abs(integral - integral_prev) > 15*E);

        double error = Math.Abs((integral_prev - integral) / (Math.Pow(0.5, 4) - 1));
        Console.WriteLine("error: " + error);
        Console.WriteLine("result: " + integral);
        return integral;
    }

    static double SimpsonsCubatureMethod(double a,  double b, double c, double d,  double e)
    {
        int n = 10;
        int m = 10;
        double h_x = (b - a) / (2 * n);
        double h_y = (d - c) / (2 * m);

        double integral = 0.0;
        double integral_prev = 0.0;

        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                integral += Func2(2 * i, 2 * j, n) + Func2(2 * i + 2, 2 * j, n) + Func2(2 * i + 2, 2 * j + 2, n) + Func2(2 * i, 2 * j + 2, n) +
                    4 * (Func2(2 * i + 1, 2 * j, n) + Func2(2 * i + 2, 2 * j + 1, n) + Func2(2 * i + 1, 2 * j + 2, n) + Func2(2 * i, 2 * j + 1, n)) +
                    16 * Func2(2 * i + 1, 2 * j + 1, n);
            }
        }
        integral *= h_x * h_y / 9;

        do
        {
            n *= 2;
            h_x = (b - a) / (2 * n);
            h_y = (d - c) / (2 * m);
            integral_prev = integral;
            integral = 0;
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    integral += Func2(2 * i, 2 * j, n) + Func2(2 * i + 2, 2 * j, n) + Func2(2 * i + 2, 2 * j + 2, n) + Func2(2 * i, 2 * j + 2, n) +
                        4 * (Func2(2 * i + 1, 2 * j, n) + Func2(2 * i + 2, 2 * j + 1, n) + Func2(2 * i + 1, 2 * j + 2, n) + Func2(2 * i, 2 * j + 1, n)) +
                        16 * Func2(2 * i + 1, 2 * j + 1, n);
                }
            }
            integral *= h_x * h_y / 9;
        }
        while (Math.Abs(integral - integral_prev) > 15 * e);

        Console.WriteLine("- Result: " + integral);
        return integral;
    }
    public static void Main(string[] args)
    {
        const double E1 = 1e-4;
        const double E2 = 1e-5;

        double a = 1.2;
        double b = 2.471;

        double A = -1, B = 1;
        double C = -1, D = 1;

        Console.WriteLine("enter the number of subintervals");
        int n = int.Parse(Console.ReadLine());


        Console.WriteLine("E ="+E1);
        Console.WriteLine("TrapezoidMethod: " + TrapezoidMethod(a, b, n, E1) + "\n");
        Console.WriteLine("Simpson's method: " + SimpsonMethod(a, b,n, E1) + "\n");
        Console.WriteLine("Simpson's Cubature MethoD" + SimpsonsCubatureMethod(A, B, C,D, E1) + "\n");

        Console.WriteLine("E =" + E2);
        Console.WriteLine("TrapezoidMethod: " + TrapezoidMethod(a, b, n, E2) + "\n");
        Console.WriteLine("Simpson's method: " + SimpsonMethod(a, b, n, E2) + "\n");
        Console.WriteLine("Simpson's Cubature MethoD" + SimpsonsCubatureMethod(A, B, C, D, E2) + "\n");

    }
}