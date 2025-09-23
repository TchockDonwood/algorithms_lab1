using System;
using System.Diagnostics;
using ScottPlot;
using MathNet.Numerics;

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
            /// Список замеров времени в миллисекундах (значения для оси Y).
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
    }
}