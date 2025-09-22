// Program.cs
using System;
using System.Collections.Generic;

namespace Algorithms_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            Benchmark.Run(
                testAction: n => 
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++) 
                        arr[i] = rnd.NextDouble();

                    Algorithms.ConstFunction(arr);
                },
                "ConstFunction", startN: 1, endN: 2000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n => 
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++) 
                        arr[i] = rnd.NextDouble();

                    Algorithms.SumFunction(arr);
                },
                "SumFunction", startN: 1, endN: 10000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.NextDouble();

                    Algorithms.ProductFunction(arr);
                },
                "ProductFunction", startN: 1, endN: 10000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.NextDouble();

                    Algorithms.NaivePolynomial(arr, 1.5);
                },
                "NaivePolynomial", startN: 1, endN: 5000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.NextDouble();

                    Algorithms.HornerPolynomial(arr, 1.5);
                },
                "HornerPolynomial", startN: 1, endN: 5000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.NextDouble();

                    Algorithms.BubbleSort(arr);
                },
                "BubbleSort", startN: 1, endN: 2000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.NextDouble();

                    Algorithms.TimSort(arr);
                },
                "TimSort", startN: 1, endN: 5000, step: 1
            );

            //------

            Benchmark.Run(
               testAction: n => {
                   var matrixA = new Matrix(n, n * 2);
                   var matrixB = new Matrix(n * 2, n);
                   matrixA.FillRandom();
                   matrixB.FillRandom();
                   Algorithms.MultiplyMatrix(matrixA, matrixB);
               },
               "MultiplyMatrix", startN: 10, endN: 500, step: 10
           );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.Next();

                    Algorithms.HeapSort(arr);
                },
                "HeapSort", startN: 1, endN: 2000, step: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var chars = new char[n];

                    for (int i = 0; i < n; i++)
                        chars[i] = (char)('0' + rnd.Next(0, 10));

                    Algorithms.GetUniqueSubstrings(new string(chars));
                },
                "GetUniqueSubstrings", startN: 1, endN: 250, step: 10
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.NextDouble();

                    Algorithms.ShellSort(arr);
                },
                "ShellSort", startN: 1, endN: 2000, step: 10
            );

            //------
           
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];

                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = rnd.Next();

                    Algorithms.MergeSort(arr);
                },
                "MergeSort", startN: 1, endN: 2000, step: 1
            );

            //------

            Console.WriteLine("\nВсе замеры завершены.");
        }
    }
}
