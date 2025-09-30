using System;
using System.Diagnostics;
using ScottPlot;
using MathNet.Numerics;
using Plotly.NET;
using Plotly.NET.TraceObjects;
using Plotly.NET.ImageExport;
using System.Reflection.Emit;
using static Algorithms_lab1.StepBenchmark;



namespace Algorithms_lab1
{
    public static class Benchmark
    {
        public class BenchmarkResult
        {
            public string Label { get; }

            public List<double> Ns { get; set; } = new List<double>();

            public List<double> Ms { get; } = new List<double>();

            public List<double> Times { get; set; } = new List<double>();

            public BenchmarkResult(string label)
            {
                Label = label;
            }
        }

        public static double Median(IList<double> values)
        {
            var sorted = values.OrderBy(x => x).ToList();
            int count = sorted.Count;

            if (count % 2 == 1)
            {
                return sorted[count / 2];
            }
            else
            {
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            }
        }

        private static double Median(double[] sortedArray, int start, int end)
        {
            int count = end - start + 1;
            int mid = start + count / 2;
            if (count % 2 == 0)
            {
                return (sortedArray[mid - 1] + sortedArray[mid]) / 2.0;
            }
            return sortedArray[mid];
        }

        public static (List<double> filteredX, List<double> filteredY) FilterIqrOutliers(double[] xData, double[] yData, double factor = 1)
        {
            double[] sortedY = yData.OrderBy(y => y).ToArray();
            int n = sortedY.Length;

            // 2. Находим Q1 (25-й перцентиль) и Q3 (75-й перцентиль).
            // Для простоты берем медиану нижней и верхней половины данных.
            int midIndex = n / 2;

            // Нижняя половина (для Q1)
            double q1 = Median(sortedY, 0, midIndex - 1);

            // Верхняя половина (для Q3)
            double q3 = Median(sortedY, n - midIndex, n - 1);

            // 3. Вычисляем IQR и границы для выбросов.
            double iqr = q3 - q1;
            double lowerBound = q1 - factor * iqr;
            double upperBound = q3 + factor * iqr;

            // 4. Проходим по оригинальным данным и отбираем те, что не являются выбросами.
            var filteredX = new List<double>();
            var filteredY = new List<double>();

            for (int i = 0; i < yData.Length; i++)
            {
                if (yData[i] >= lowerBound && yData[i] <= upperBound)
                {
                    filteredX.Add(xData[i]);
                    filteredY.Add(yData[i]);
                }
            }

            Console.WriteLine($"IQR Фильтр: Q1={q1:F4}, Q3={q3:F4}, Границы=[{lowerBound:F4}, {upperBound:F4}]. Отфильтровано {yData.Length - filteredY.Count} точек.");

            return (filteredX, filteredY);
        }

        public static void Run(Action<int> testAction, string label, int startN, int endN, int step = 1, int repetitions = 50, int scale = 0, double factor = 1)
        {
            if (testAction == null) throw new ArgumentNullException(nameof(testAction));
            if (startN <= 0 || endN <= 0 || step <= 0 || repetitions <= 0)
                throw new ArgumentException("Параметры размеров и повторений должны быть положительными.");

            var result = new BenchmarkResult(label);
            var stopwatch = new Stopwatch();

            Console.WriteLine($"--- Запуск бенчмарка для '{label}' ---");

            for (int n = startN; n <= endN; n += step)
            {
                testAction(n);

                var samples = new List<double>();

                for (int i = 0; i < repetitions; i++)
                {
                    stopwatch.Restart();
                    testAction(n);
                    stopwatch.Stop();

                    samples.Add(stopwatch.Elapsed.TotalMilliseconds);
                }

                double medianTime = Median(samples);

                result.Ns.Add(n);
                result.Times.Add(medianTime);

                Console.WriteLine($"N = {n}, Медианное время: {medianTime:F4} мс");
            }
            Console.WriteLine("------------------------------------------\n");

            // --- Убираем всплески ---
            (result.Ns, result.Times) = FilterIqrOutliers(result.Ns.ToArray(), result.Times.ToArray(), factor);

            // --- Аппроксимация ---
            double[] xData = result.Ns.ToArray();
            double[] yData = result.Times.ToArray();

            var models = new Dictionary<string, Func<double, double>>
            {
                { "O(1)", n => 1 },
                { "O(log n)", n => Math.Log(n) },
                { "O(n)", n => n },
                { "O(n log n)", n => n * Math.Log(n) },
                { "O(n^2)", n => n * n },
                { "O(n^3)", n => n * n * n }
            };

            string bestModel = null;
            double bestError = double.MaxValue;
            double[] bestFit = null;

            foreach (var kv in models)
            {
                double[] transformedX = xData.Select(kv.Value).ToArray();
                double constantFactor = Fit.LineThroughOrigin(transformedX, yData);
                double[] yFit = xData.Select(x => constantFactor * kv.Value(x)).ToArray();

                double mse = 0;

                for (int i = 0; i < yData.Length; i++)
                {
                    double diff = yData[i] - yFit[i];
                    mse += diff * diff;
                }

                mse /= yData.Length;
                              
                if (mse < bestError)
                {
                    bestError = mse;
                    bestModel = kv.Key;
                    bestFit = yFit;
                }
            }

            Plot(new List<BenchmarkResult> { result }, label, bestFit, bestModel, scale);
        }

        public static void Run(Action<int, int> testAction, string label, int startN, int endN, int startM, int endM, int step = 1, int repetitions = 5)
        {
            if (testAction == null) throw new ArgumentNullException(nameof(testAction));

            if (startN <= 0 || endN <= 0 || startM <= 0 || endM <= 0 || step <= 0 || repetitions <= 0)
                throw new ArgumentException("Параметры размеров и повторений должны быть положительными.");

            var result = new BenchmarkResult(label);
            var stopwatch = new Stopwatch();

            Console.WriteLine($"--- Запуск бенчмарка для '{label}' ---");

            for (int n = startN; n <= endN; n += step)
            {
                for (int m = startM; m <= endM; m += step)
                {
                    testAction(n, m);

                    var samples = new List<double>();

                    for (int i = 0; i < repetitions; i++)
                    {
                        stopwatch.Restart();
                        testAction(n, m);
                        stopwatch.Stop();

                        samples.Add(stopwatch.Elapsed.TotalMilliseconds);
                    }

                    double medianTime = Median(samples);

                    result.Ns.Add(n);
                    result.Ms.Add(m);
                    result.Times.Add(medianTime);

                    Console.WriteLine($"N = {n}, M = {m}, Медианное время: {medianTime:F4} мс");
                }            
            }
            Console.WriteLine("------------------------------------------\n");

            var r = (endN - startN + 1) / step;
            var c = (endM - startM + 1) / step;

            double[,] z = new double[r, c];

            var index = 0;

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    z[i, j] = result.Times[index];
                    index++;
                }
            }

            DisplayMatrix3D(z, label);
        }

        static void Plot(List<BenchmarkResult> results, string title, double[] fittedData, string approximation, int scale)
        {
            if (results == null || results.Count == 0)
            {
                Console.WriteLine("Нет данных для построения графика.");
                return;
            }
            
            var filePath = $"{title}.png";

            var plt = new Plot();
            plt.Title(title);
            plt.XLabel("Размер входных данных (N)");
            plt.YLabel("Время выполнения (мс)");

            foreach (var result in results)
            {
                double[] xData = result.Ns.ToArray();
                double[] yData = result.Times.ToArray();

                if (xData.Length > 0)
                {
                    var scatter = plt.Add.Scatter(xData, yData);
                    scatter.LegendText = "Эксперементальные результаты";
                    scatter.MarkerShape = MarkerShape.None;
                }

                if (fittedData != null && fittedData.Length == xData.Length)
                {
                    var scatterFit = plt.Add.Scatter(xData, fittedData);
                    scatterFit.LegendText = $"Аппроксимация: {approximation}";
                    scatterFit.MarkerShape = MarkerShape.None;
                    scatterFit.LineStyle.Color = Colors.Red;
                    scatterFit.LineStyle.Width = 2;
                }
            }

            if (scale != 0)
            {
                plt.Axes.SetLimitsY(0, scale);
            }

            plt.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            plt.SavePng(filePath, 800, 600);

            Console.WriteLine($"График сохранен в файл: {System.IO.Path.GetFullPath(filePath)}");
        }

        public static void DisplayMatrix3D(double[,] z, string title)
        {
            var filePath = $"{title}.png";

            var trace = new Plotly.NET.Trace("surface");
            trace.SetValue("z", z);
            trace.SetValue("type", "surface");

            var chart = GenericChart.ofTraceObject(false, trace);

            var layout = new Plotly.NET.Layout();
            layout.SetValue("title", title);
            layout.SetValue("autosize", true);
            layout.SetValue("width", 1200);
            layout.SetValue("height", 900);
            
            layout.SetValue("margin", new { l = 0, r = 0, b = 0, t = 50, pad = 4 });

            layout.SetValue("scene", new
            {
                xaxis = new { title = "Размер M" },
                yaxis = new { title = "Размер N" },
                zaxis = new { title = "Время выполнения (мс)" },

                aspectratio = new { x = 1, y = 1, z = 0.7},
                camera = new
                {
                    eye = new { x = -1.5, y = -1.5, z = 1.5 }
                }
            });

            chart.WithLayout(layout);
            chart.SaveHtml(filePath);
            chart.SavePNG(filePath);

            Console.WriteLine($"График сохранен в файл: {System.IO.Path.GetFullPath(filePath)}");
        }

        public static void CompareAlgorithms(List<Action<int>> testActions, string[] labels, int startN, int endN, int step = 1, int repetitions = 50)
        {
            if (testActions == null) throw new ArgumentNullException(nameof(testActions));
            if (labels == null || labels.Length != testActions.Count) throw new ArgumentNullException();
            if (startN <= 0 || endN <= 0 || step <= 0 || repetitions <= 0)
                throw new ArgumentException("Параметры размеров и повторений должны быть положительными.");

            var results = new List<BenchmarkResult>();
            var stopwatch = new Stopwatch();
            
            var index = 0;
            foreach (var testAction in testActions)
            {
                var label = labels[index];

                var result = new BenchmarkResult(label);

                Console.WriteLine($"--- Запуск бенчмарка для '{label}' ---");

                for (int n = startN; n <= endN; n += step)
                {
                    testAction(n);

                    var samples = new List<double>();

                    for (int i = 0; i < repetitions; i++)
                    {
                        stopwatch.Restart();
                        testAction(n);
                        stopwatch.Stop();

                        samples.Add(stopwatch.Elapsed.TotalMilliseconds);
                    }

                    double medianTime = Median(samples);

                    result.Ns.Add(n);
                    result.Times.Add(medianTime);

                    Console.WriteLine($"N = {n}, Медианное время: {medianTime:F4} мс");
                }
                Console.WriteLine("------------------------------------------\n");

                // --- Убираем всплески ---
                (result.Ns, result.Times) = FilterIqrOutliers(result.Ns.ToArray(), result.Times.ToArray());

                results.Add(result);

                index++;
            }

            // Построение общего графика
            PlotComparison(results, "Сравнение алгоритмов", "algorithms_comparison.png");
        }

        static void PlotComparison(List<BenchmarkResult> results, string title, string filePath)
        {
            var plt = new Plot();
            plt.Title(title);
            plt.XLabel("Размер входных данных (N)");
            plt.YLabel("Время выполнения (мс)");

            var colors = new[] { Colors.Blue, Colors.Red };

            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                double[] xData = result.Ns.ToArray();
                double[] yData = result.Times.ToArray();

                if (xData.Length > 0)
                {
                    var scatter = plt.Add.Scatter(xData, yData);
                    scatter.LegendText = result.Label;
                    scatter.MarkerShape = MarkerShape.None;
                    scatter.LineStyle.Color = colors[i % colors.Length];
                    scatter.LineStyle.Width = 2;
                }
            }

            plt.Axes.SetLimitsY(0, 15);

            plt.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            plt.SavePng(filePath, 800, 600);
            Console.WriteLine($"Сравнительный график сохранен: {System.IO.Path.GetFullPath(filePath)}");
        }
    }
}