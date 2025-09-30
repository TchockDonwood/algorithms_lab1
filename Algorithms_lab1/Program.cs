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
            StepBenchmark.Run(
                testAction: n => Algorithms.PowWithSteps(2, n).steps,
                label: "Pow",
                startN: 1, endN: 100_000, step: 100
            );

            StepBenchmark.Run(
                testAction: n => Algorithms.RecPowWithSteps(2, n).steps,
                label: "RecPow",
                startN: 1, endN: 100_000, step: 100
            );

            StepBenchmark.Run(
                testAction: n => Algorithms.QuickPowWithSteps(2, n).steps,
                label: "QuickPow",
                startN: 1, endN: 100_000, step: 100
            );

            StepBenchmark.Run(
                testAction: n => Algorithms.ClassicalQuickPowWithSteps(2, n).steps,
                label: "ClassicalQuickPow",
                startN: 1, endN: 100_000, step: 100
            );

            StepBenchmark.CompareAllPowerAlgorithms(1, 10_000, 10);
            */

            Benchmark.CompareAlgorithms(
                new List<Action<int>>
                {
                    n =>
                    {
                        var arr = new int[n];
                        for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                        Algorithms.SumFunction(arr);
                    },
                    n => 
                    {
                        var arr = new int[n];
                        for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                        Algorithms.ProductFunction(arr);
                    }
                },
                labels: new string[] { "SumFunction", "ProductFunction" },
                startN: 1, endN: 2_000_000, step: 5000
            );
            

            //------
            /*
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.QuickSort(arr, 0, n - 1);
                },
                label: "QuickSort",
                startN: 1, endN: 500_000, step: 5000
            );
            
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    Algorithms.ConstFunction(arr);
                },
                label: "ConstFunction",
                startN: 1, endN: 1_000_000, step: 1_000,
                scale: 5,
                factor: 0.5
            );

            //------
            
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                    Algorithms.SumFunction(arr);
                },
                label: "SumFunction",
                startN: 1, endN: 1_000_000, step: 2500,
                scale: 15,
                factor: 1
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                    Algorithms.ProductFunction(arr);
                },
                label: "ProductFunction",
                startN: 1, endN: 1_000_000, step: 2500,
                scale: 15,
                factor: 1
            );
            
            //------
            
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next() * rnd.NextDouble();
                    Algorithms.NaivePolynomial(arr, 1.5);
                },
                label: "NaivePolynomial",
                startN: 1, endN: 500_000, step: 2500
            );

            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next() * rnd.NextDouble();
                    Algorithms.HornerPolynomial(arr, 1.5);
                },
                label: "HornerPolynomial",
                startN: 1, endN: 500_000, step: 2500
            );
            /*
            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.BubbleSort(arr);
                },
                label: "BubbleSort",
                startN: 1, endN: 50_000, step: 2500
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
                startN: 1, endN: 100_000, step: 1000
            );

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
                startN: 1, endN: 450, 
                startM: 1, endM: 450,
                step: 10
            );
            
            //------

            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new int[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next();
                    Algorithms.HeapSort(arr);
                },
                label: "HeapSort",
                startN: 1, endN: 500_000, step: 5000
            );

            //------
            */
            Benchmark.Run(
                testAction: n =>
                {
                    var chars = new char[n];
                    for (int i = 0; i < n; i++) chars[i] = (char)('0' + rnd.Next(0, 10));
                    Algorithms.GetUniqueSubstrings(new string(chars));
                },
                label: "GetUniqueSubstrings",
                startN: 1, endN: 300, step: 5
            );

            //------
/*
            Benchmark.Run(
                testAction: n =>
                {
                    var arr = new double[n];
                    for (int i = 0; i < arr.Length; i++) arr[i] = rnd.NextDouble();
                    Algorithms.ShellSort(arr);
                },
                label: "ShellSort",
                startN: 1, endN: 500_000, step: 5000
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
                startN: 1, endN: 500_000, step: 5000
            );

            //------
*/
            Console.WriteLine("\nВсе замеры завершены.");
        }
    }
}