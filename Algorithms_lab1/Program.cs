using System;

public class Program
{
    public static void Main()
    {
        
    }
}

public class Matrix
{
    private int[,] data;
    public int Rows { get; }
    public int Cols { get; }

    public Matrix()
    {
        Rows = 0;
        Cols = 0;
        data = new int[0, 0];
    }

    public Matrix(int n, int m)
    {
        if (n <= 0 || m <= 0)
            throw new ArgumentException("Размеры матрицы должны быть положительными");

        Rows = n;
        Cols = m;
        data = new int[n, m];
    }

    public int this[int i, int j]
    {
        get => data[i, j];
        set => data[i, j] = value;
    }

    public void FillRandom()
    {
        var rnd = new Random();

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
                data[i, j] = rnd.Next();
        }
    }

    public void Print()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
                Console.Write(data[i, j] + " ");
            Console.WriteLine();
        }
    }
}

public class AlgorithmsBencmark
{ 
    // TODO: Доделать алгоритмы 1-7
    // Для вектора сделать только алгоритмы, инициализировать и заполнять значениями нужно в Main

    // Алгоритм умножения матриц

    public static int[,] MultiplyMatrix(int[,] matrixA, int[,] matrixB)
    {
        var n = matrixA.GetLength(0);
        var m = matrixA.GetLength(1);
        var b = matrixB.GetLength(1);

        var result = new int[n, b];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < b; j++)
            {
                var sum = 0;

                for (int k = 0; k < m; k++)
                {
                    sum += matrixA[i, k] * matrixB[k, j];
                }

                result[i, j] = sum;
            }
        }

        return result;
    }

    // Алгоритмы возведения в степень

    public static long Pow(long x, int n)
    {
        if (n < 0)
            throw new ArgumentException("Степень должна быть неотрицательной!", nameof(n));

        long result = 1;

        for (int i = 0; i < n; i++)
            result *= x;

        return result;
    }

    public static long RecPow(long x, int n)
    {
        if (n < 0)
            throw new ArgumentException("Степень должна быть неотрицательной!", nameof(n));

        if (n == 0)
            return 1;

        long result = RecPow(x, n / 2);

        if (n % 2 != 0)
            return result * result * x;
        else
            return result * result;
    }

    public static long QuickPow(long x, int n)
    {
        if (n < 0)
            throw new ArgumentException("Степень должна быть неотрицательной!", nameof(n));

        long result = 1;
        long baseVal = x;
        int exponent = n;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result *= baseVal;

            baseVal *= baseVal;
            exponent /= 2;
        }

        return result;
    }

    public static long ClassicalQuickPow(long x, int n)
    {
        if (n < 0)
            throw new ArgumentException("Степень должна быть неотрицательной!", nameof(n));

        long result = 1;
        long baseVal = x;
        int exponent = n;

        while (exponent > 0)
        {
            if (exponent % 2 == 0)
            {
                baseVal *= baseVal;
                exponent /= 2;
            }
            else
            {
                result *= baseVal;
                exponent--;
            }
        }

        return result;
    }

    // TODO: Доделать алгоритмы сложности > O(n)
}
