using System;
using System.Collections.Generic;

namespace Algorithms_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            /*
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.QuickSort(arr, 0, n - 1);
                },
                label: "QuickSort",
                complexityFunction: n => n * n, // O(N^2)
                startN: 1, endN: 100_000, step: 10
            );


            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.ConstFunction(arr);
                },
                label: "ConstFunction",
                complexityFunction: n => 1, // O(1)
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.SumFunction(arr);
                },
                label: "SumFunction",
                complexityFunction: n => n, // O(N)
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.ProductFunction(arr);
                },
                label: "ProductFunction",
                complexityFunction: n => n, // O(N)
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.NaivePolynomial(arr, 1.5);
                },
                label: "NaivePolynomial",
                complexityFunction: n => n * n, // O(N^2)
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.HornerPolynomial(arr, 1.5);
                },
                label: "HornerPolynomial",
                complexityFunction: n => n, // O(N)
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.BubbleSort(arr);
                },
                label: "BubbleSort",
                complexityFunction: n => n * n, // O(N^2)
                startN: 1, endN: 2000, step: 10
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.TimSort(arr);
                },
                label: "TimSort",
                complexityFunction: n => n * Math.Log(n), // O(N log N)
                startN: 1, endN: 100_000, step: 50
            );
            */
            //------

            Benchmark.Run(
                testAction: (n, m) => {
                    var matrixA = new Matrix(n, m);
                    var matrixB = new Matrix(m, n);
                    matrixA.FillRandom();
                    matrixB.FillRandom();
                    Algorithms.MultiplyMatrix(matrixA, matrixB);
                },
                label: "MultiplyMatrix",
                startN: 1, endN: 100, 
                startM: 1, endM: 100,
                step: 10
            );

            //------
            /*
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                    Algorithms.HeapSort(arr);
                },
                label: "HeapSort",
                complexityFunction: n => n * Math.Log(n), // O(N log N)
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var chars = new char[n];
                    for (int i = 0; i < n; i++) chars[i] = (char)('0' + rnd.Next(0, 10));
                    Algorithms.GetUniqueSubstrings(new string(chars));
                },
                label: "GetUniqueSubstrings",
                complexityFunction: n => Math.Pow(n, 3), // O(N^3)
                startN: 1, endN: 300, step: 10
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.ShellSort(arr);
                },
                label: "ShellSort",
                complexityFunction: n => Math.Pow(n, 1.5), // O(N^1.5), зависит от последовательности шагов
                startN: 1, endN: 100_000, step: 50
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                    Algorithms.MergeSort(arr);
                },
                label: "MergeSort",
                complexityFunction: n => n * Math.Log(n), // O(N log N)
                startN: 1, endN: 100_000, step: 50
            );

            //------
            */
            Console.WriteLine("\nВсе замеры завершены.");

        }
    }
}