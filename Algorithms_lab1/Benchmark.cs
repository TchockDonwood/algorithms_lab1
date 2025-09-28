using System;
using System.Diagnostics;
using ScottPlot;
using MathNet.Numerics;
using Plotly.NET;
using Plotly.NET.TraceObjects;
using Plotly.NET.ImageExport;
using static Plotly.NET.StyleParam.Range;
//using Plotly.NET.CSharp;

namespace Algorithms_lab1
{
    public static class Benchmark
    {
        /// <summary>
        /// Класс для хранения результатов одного запуска бенчмарка.
        /// </summary>
        public class BenchmarkResult
        {
            /// <summary>
            /// Название алгоритма/теста.
            /// </summary>
            public string Label { get; }

            /// <summary>
            /// Список размеров входных данных (значения для оси X).
            /// </summary>
            public List<double> Ns { get; } = new List<double>();

            /// <summary>
            /// Список размеров входных данных (значения для оси Y в 3-х мерных графиках).
            /// </summary>
            public List<double> Ms { get; } = new List<double>();

            /// <summary>
            /// Список замеров времени в миллисекундах (значения для оси Y (для 2-х мерных графиков) и значения для оси Z (для 3-х мерных графиков)).
            /// </summary>
            public List<double> Times { get; } = new List<double>();

            public BenchmarkResult(string label)
            {
                Label = label;
            }
        }

        /// <summary>
        /// Выполняет замер производительности для заданного действия.
        /// </summary>
        /// <param name="testAction">Действие для тестирования. Принимает на вход размер данных (int n).</param>
        /// <param name="startN">Начальный размер данных.</param>
        /// <param name="endN">Конечный размер данных.</param>
        /// <param name="step">Шаг изменения размера данных.</param>
        /// <param name="repetitions">Количество повторений для каждого размера данных, чтобы получить среднее время.</param>
        /// <returns>Объект с результатами замеров.</returns>
        public static void Run(Action<int> testAction, string label, Func<double, double> complexityFunction, int startN, int endN, int step = 1, int repetitions = 5)
        {
            if (testAction == null) throw new ArgumentNullException(nameof(testAction));
            if (startN <= 0 || endN <= 0 || step <= 0 || repetitions <= 0)
                throw new ArgumentException("Параметры размеров и повторений должны быть положительными.");

            var result = new BenchmarkResult(label);
            var stopwatch = new Stopwatch();

            Console.WriteLine($"--- Запуск бенчмарка для '{label}' ---");

            for (int n = startN; n <= endN; n += step)
            {
                // "Прогрев" JIT-компилятора
                // Первый запуск может быть дольше из-за компиляции, поэтому его не замеряем.
                testAction(n);

                // Запускаем замеры
                stopwatch.Restart();
                for (int i = 0; i < repetitions; i++)
                {
                    testAction(n);
                }
                stopwatch.Stop();

                // Считаем среднее время в миллисекундах
                double averageTime = stopwatch.Elapsed.TotalMilliseconds / repetitions;

                result.Ns.Add(n);
                result.Times.Add(averageTime);

                Console.WriteLine($"N = {n}, Среднее время: {averageTime:F4} мс");
            }
            Console.WriteLine("------------------------------------------\n");

            // --- Аппроксимация ---
            double[] xData = result.Ns.ToArray();
            double[] yData = result.Times.ToArray();

            // преобразуем N через функцию сложности
            double[] transformedX = xData.Select(complexityFunction).ToArray();

            // аппроксимация линейной зависимостью через начало координат
            double constantFactor = Fit.LineThroughOrigin(transformedX, yData);

            double[] yFit = xData.Select(x => constantFactor * complexityFunction(x)).ToArray();


            Plot(new List<BenchmarkResult> { result }, label, $"{label}.png", yFit);
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
                    // "Прогрев" JIT-компилятора
                    // Первый запуск может быть дольше из-за компиляции, поэтому его не замеряем.
                    testAction(n, m);

                    // Запускаем замеры
                    stopwatch.Restart();
                    for (int i = 0; i < repetitions; i++)
                    {
                        testAction(n, m);
                    }
                    stopwatch.Stop();

                    // Считаем среднее время в миллисекундах
                    double averageTime = stopwatch.Elapsed.TotalMilliseconds / repetitions;

                    result.Ns.Add(n);
                    result.Ms.Add(m);
                    result.Times.Add(averageTime);

                    Console.WriteLine($"N = {n}, M = {m}, Среднее время: {averageTime:F4} мс");
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

            DisplayMatrix3D(z, label, $"{label}.png");
        }

        /// <summary>
        /// Строит и сохраняет график на основе одного или нескольких результатов бенчмарка.
        /// </summary>
        /// <param name="results">Список результатов для построения.</param>
        /// <param name="title">Заголовок графика.</param>
        /// <param name="filePath">Путь для сохранения файла (например, "my_plot.png").</param>
        static void Plot(List<BenchmarkResult> results, string title, string filePath, double[] fittedData)
        {
            if (results == null || results.Count == 0)
            {
                Console.WriteLine("Нет данных для построения графика.");
                return;
            }

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
                    scatterFit.LegendText = "Аппроксимация";
                    scatterFit.MarkerShape = MarkerShape.None;
                    scatterFit.LineStyle.Color = Colors.Red;
                    scatterFit.LineStyle.Width = 2;
                }
            }

            plt.ShowLegend(Alignment.UpperLeft, Orientation.Horizontal);
            plt.SavePng(filePath, 800, 600);

            Console.WriteLine($"График сохранен в файл: {System.IO.Path.GetFullPath(filePath)}");
        }

        public static void DisplayMatrix3D(double[,] z, string title, string filePath)
        {

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
    }
}