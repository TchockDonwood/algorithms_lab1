// Program.cs
using System;
using System.Collections.Generic;

namespace Algorithms_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrixResults = new List<Benchmark.BenchmarkResult>();
            var result = Benchmark.Run(
                label: "Умножение матриц",
                testAction: n => {
                    var matrixA = new Matrix(n, n);
                    var matrixB = new Matrix(n, n);
                    matrixA.FillRandom();
                    matrixB.FillRandom();
                    Algorithms.MultiplyMatrix(matrixA, matrixB);
                },
                startN: 10, endN: 500, step: 10, repetitions: 5
            );
            matrixResults.Add(result);

            // Коэффициент 'c' нужно подобрать экспериментально,
            // чтобы график аппроксимации был близок к реальным замерам.
            // c ≈ T(n) / n^3
            // Для N=100, T(100) ≈ 25 мс.  c ≈ 25 / 100^3 = 0.000025
            double c = 0.000025;
            Func<double, double> cubicApproximation = n => c * Math.Pow(n, 3);

            Benchmark.Plot(
                matrixResults,
                "Производительность умножения матриц (N x N)",
                "matrix_multiplication_approx.png",
                cubicApproximation, // Передаем нашу функцию
                "Аппроксимация O(n^3)" // И ее название для легенды
            );

            // --- Пример 1: Сравнение алгоритмов возведения в степень ---
            var powerResults = new List<Benchmark.BenchmarkResult>();

            // Для очень быстрых операций нужно много повторений, чтобы замер был точным.
            powerResults.Add(Benchmark.Run(
                label: "Pow (итеративный)",
                testAction: n => Algorithms.Pow(2, n),
                startN: 1000, endN: 10000, step: 1000, repetitions: 1000
            ));

            powerResults.Add(Benchmark.Run(
                label: "QuickPow (быстрое)",
                testAction: n => Algorithms.QuickPow(2, n),
                startN: 100000, endN: 1000000, step: 100000, repetitions: 1000
            ));

            Benchmark.Plot(powerResults, "Сравнение алгоритмов возведения в степень", "power_comparison.png");

            /*
            // --- Пример 2: Замер умножения матриц ---
            var matrixResults = new List<Benchmark.BenchmarkResult>();

            matrixResults.Add(Benchmark.Run(
                label: "Умножение матриц",
                testAction: n => {
                    // Создаем матрицы внутри, чтобы на каждом шаге были новые данные.
                    // Заполнение матриц не входит во время замера самого алгоритма.
                    var matrixA = new Matrix(n, n);
                    var matrixB = new Matrix(n, n);
                    matrixA.FillRandom();
                    matrixB.FillRandom();
                    Algorithms.MultiplyMatrix(matrixA, matrixB);
                },
                startN: 10, endN: 100, step: 10, repetitions: 3 // Алгоритм медленный, много повторений не нужно
            ));

            Benchmark.Plot(matrixResults, "Производительность умножения матриц (N x N)", "matrix_multiplication.png");
            */

            Console.WriteLine("\nВсе замеры завершены.");
        }
    }
}
