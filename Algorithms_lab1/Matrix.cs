using System;
using Algorithms_lab1;

namespace Algorithms_lab1
{
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
            if (n < 0 || m < 0)
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
}
