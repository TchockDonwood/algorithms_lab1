using System;
using System.Collections.Generic;
using System.Linq;
using ScottPlot;

namespace Algorithms_lab1
{
    public static class StepBenchmark
    {
        public class StepBenchmarkResult
        {
            public string Label { get; }
            public List<double> Ns { get; } = new List<double>();
            public List<double> StepCounts { get; } = new List<double>();

            public StepBenchmarkResult(string label)
            {
                Label = label;
            }
        }

        public static void Run(Func<int, long> testAction, string label, int startN, int endN, int step = 1)
        {
            if (testAction == null) throw new ArgumentNullException(nameof(testAction));
            if (startN <= 0 || endN <= 0 || step <= 0)
                throw new ArgumentException("Параметры размеров должны быть положительными.");

            var result = new StepBenchmarkResult(label);

            Console.WriteLine($"--- Запуск подсчёта шагов для '{label}' ---");

            for (int n = startN; n <= endN; n += step)
            {
                long steps = testAction(n);
                result.Ns.Add(n);
                result.StepCounts.Add(steps);

                Console.WriteLine($"n = {n}, шагов: {steps}");
            }
            Console.WriteLine("------------------------------------------\n");

            Plot(new List<StepBenchmarkResult> { result }, label);
        }

        static void Plot(List<StepBenchmarkResult> results, string title)
        {
            if (results == null || results.Count == 0)
            {
                Console.WriteLine("Нет данных для построения графика.");
                return;
            }

            var filePath = $"{title}_steps.png";

            var plt = new Plot();
            plt.Title($"Зависимость шагов от n: {title}");
            plt.XLabel("Показатель степени (n)");
            plt.YLabel("Количество шагов");

            foreach (var result in results)
            {
                double[] xData = result.Ns.ToArray();
                double[] yData = result.StepCounts.ToArray();

                if (xData.Length > 0)
                {
                    var scatter = plt.Add.Scatter(xData, yData);
                    scatter.LegendText = "Экспериментальные результаты";
                    scatter.MarkerShape = MarkerShape.None;
                }
            }

            plt.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            plt.SavePng(filePath, 800, 600);

            Console.WriteLine($"График сохранен в файл: {System.IO.Path.GetFullPath(filePath)}");
        }

        public static void CompareAllPowerAlgorithms(int startN, int endN, int step = 1)
        {
            var results = new List<StepBenchmarkResult>();
            long baseNumber = 2;

            // Тестируем все алгоритмы
            var algorithms = new[]
            {
                new { Name = "Pow", Function = new Func<int, long>(n => Algorithms.PowWithSteps(baseNumber, n).steps) },
                new { Name = "RecPow", Function = new Func<int, long>(n => Algorithms.RecPowWithSteps(baseNumber, n).steps) },
                new { Name = "QuickPow", Function = new Func<int, long>(n => Algorithms.QuickPowWithSteps(baseNumber, n).steps) },
                new { Name = "ClassicalQuickPow", Function = new Func<int, long>(n => Algorithms.ClassicalQuickPowWithSteps(baseNumber, n).steps) }
            };

            foreach (var algo in algorithms)
            {
                var result = new StepBenchmarkResult(algo.Name);
                Console.WriteLine($"--- Подсчёт шагов для {algo.Name} ---");

                for (int n = startN; n <= endN; n += step)
                {
                    long steps = algo.Function(n);
                    result.Ns.Add(n);
                    result.StepCounts.Add(steps);
                    Console.WriteLine($"n = {n}, шагов: {steps}");
                }
                results.Add(result);
                Console.WriteLine();
            }

            // Построение общего графика
            PlotComparison(results, "Сравнение алгоритмов возведения в степень", "power_algorithms_comparison.png");
        }

        static void PlotComparison(List<StepBenchmarkResult> results, string title, string filePath)
        {
            var plt = new Plot();
            plt.Title(title);
            plt.XLabel("Показатель степени (n)");
            plt.YLabel("Количество шагов");

            var colors = new[] { Colors.Blue, Colors.Red, Colors.Green, Colors.Orange };

            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                double[] xData = result.Ns.ToArray();
                double[] yData = result.StepCounts.ToArray();

                if (xData.Length > 0)
                {
                    var scatter = plt.Add.Scatter(xData, yData);
                    scatter.LegendText = result.Label;
                    scatter.MarkerShape = MarkerShape.None;
                    scatter.LineStyle.Color = colors[i % colors.Length];
                    scatter.LineStyle.Width = 2;
                }
            }

            plt.Axes.SetLimitsY(0, 500);

            plt.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            plt.SavePng(filePath, 800, 600);
            Console.WriteLine($"Сравнительный график сохранен: {System.IO.Path.GetFullPath(filePath)}");
        }
    }
}