using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ScottPlot;

public class Program
{
    static void Main()
    {
        // Параметры по умолчанию
        int maxN = 2000;
        int step = 1;
        int repeats = 5;
        double x = 1.5;
        Random rand = new Random();

        // Выбор алгоритмов через консольное меню
        bool runNaive = false;
        bool runHorner = false;
        bool runBubble = false;
        bool runShell = false;
        bool runMatrix = false;
        bool runPow = false;
        bool runRecPow = false;
        bool runQuickPow = false;
        bool runClassicalQuickPow = false;

        Console.WriteLine("=== АНАЛИЗ АЛГОРИТМОВ ===");

        Console.WriteLine("\nНастройка параметров анализа:");

        Console.Write($"Максимальный размер n (по умолчанию {maxN}): ");
        string inputMaxN = Console.ReadLine();
        if (!string.IsNullOrEmpty(inputMaxN) && int.TryParse(inputMaxN, out int newMaxN) && newMaxN > 0)
        {
            maxN = newMaxN;
        }

        Console.Write($"Шаг увеличения n (по умолчанию {step}): ");
        string inputStep = Console.ReadLine();
        if (!string.IsNullOrEmpty(inputStep) && int.TryParse(inputStep, out int newStep) && newStep > 0)
        {
            step = newStep;
        }

        Console.Write($"Количество повторений (по умолчанию {repeats}): ");
        string inputRepeats = Console.ReadLine();
        if (!string.IsNullOrEmpty(inputRepeats) && int.TryParse(inputRepeats, out int newRepeats) && newRepeats > 0)
        {
            repeats = newRepeats;
        }

        // Выбор алгоритмов
        Console.WriteLine("\nВыберите алгоритмы для анализа (введите номера через запятую):");
        Console.WriteLine("1. Наивный метод вычисления полинома");
        Console.WriteLine("2. Метод Горнера");
        Console.WriteLine("3. Сортировка пузырьком");
        Console.WriteLine("4. Сортировка Шелла");
        Console.WriteLine("5. Умножение матриц");
        Console.WriteLine("6. Возведение в степень (итеративное)");
        Console.WriteLine("7. Возведение в степень (рекурсивное)");
        Console.WriteLine("8. Быстрое возведение в степень");
        Console.WriteLine("9. Классическое быстрое возведение в степень");
        Console.WriteLine("10. Все алгоритмы");
        Console.Write("Ваш выбор: ");

        string inputAlgorithms = Console.ReadLine();
        string[] choices = inputAlgorithms.Split(',', ' ', ';');

        foreach (string choice in choices)
        {
            if (int.TryParse(choice.Trim(), out int algorithm))
            {
                switch (algorithm)
                {
                    case 1:
                        runNaive = true;
                        break;
                    case 2:
                        runHorner = true;
                        break;
                    case 3:
                        runBubble = true;
                        break;
                    case 4:
                        runShell = true;
                        break;
                    case 5:
                        runMatrix = true;
                        break;
                    case 6:
                        runPow = true;
                        break;
                    case 7:
                        runRecPow = true;
                        break;
                    case 8:
                        runQuickPow = true;
                        break;
                    case 9:
                        runClassicalQuickPow = true;
                        break;
                    case 10:
                        runNaive = runHorner = runBubble = runShell = runMatrix =
                            runPow = runRecPow = runQuickPow = runClassicalQuickPow = true;
                        break;
                }
            }
        }

        // Если ничего не выбрано, запускаем все алгоритмы
        if (!runNaive && !runHorner && !runBubble && !runShell && !runMatrix &&
            !runPow && !runRecPow && !runQuickPow && !runClassicalQuickPow)
        {
            Console.WriteLine("Не выбрано ни одного алгоритма. Запуск всех алгоритмов...");
            runNaive = runHorner = runBubble = runShell = runMatrix =
                runPow = runRecPow = runQuickPow = runClassicalQuickPow = true;
        }

        List<int> nValues = new List<int>();
        List<double> naiveTimes = new List<double>();
        List<double> hornerTimes = new List<double>();
        List<double> bubbleTimes = new List<double>();
        List<double> shellTimes = new List<double>();
        List<double> matrixTimes = new List<double>();
        List<double> powTimes = new List<double>();
        List<double> recPowTimes = new List<double>();
        List<double> quickPowTimes = new List<double>();
        List<double> classicalQuickPowTimes = new List<double>();

        Console.WriteLine("\nЗапуск анализа алгоритмов...");
        Console.WriteLine("Параметры:");
        Console.WriteLine($"- Максимальное n: {maxN}");
        Console.WriteLine($"- Шаг: {step}");
        Console.WriteLine($"- Повторений: {repeats}");

        Console.WriteLine("Выбранные алгоритмы:");
        if (runNaive) Console.WriteLine("- Наивный метод");
        if (runHorner) Console.WriteLine("- Метод Горнера");
        if (runBubble) Console.WriteLine("- Сортировка пузырьком");
        if (runShell) Console.WriteLine("- Сортировка Шелла");
        if (runMatrix) Console.WriteLine("- Умножение матриц");
        if (runPow) Console.WriteLine("- Возведение в степень (итеративное)");
        if (runRecPow) Console.WriteLine("- Возведение в степень (рекурсивное)");
        if (runQuickPow) Console.WriteLine("- Быстрое возведение в степень");
        if (runClassicalQuickPow) Console.WriteLine("- Классическое быстрое возведение в степень");

        Console.WriteLine("\nНажмите любую клавишу для начала анализа...");
        Console.ReadKey();
        Console.WriteLine();

        for (int n = 10; n <= maxN; n += step)
        {
            double[] v = new double[n];
            for (int i = 0; i < n; i++)
            {
                v[i] = rand.NextDouble() * 100;
            }

            double naiveTotalTime = 0;
            double hornerTotalTime = 0;
            double bubbleTotalTime = 0;
            double shellTotalTime = 0;
            double matrixTotalTime = 0;
            double powTotalTime = 0;
            double recPowTotalTime = 0;
            double quickPowTotalTime = 0;
            double classicalQuickPowTotalTime = 0;

            for (int r = 0; r < repeats; r++)
            {
                if (runNaive)
                {
                    Stopwatch sw1 = Stopwatch.StartNew();
                    double resultNaive = AlgorithmsBenchmark.NaivePolynomial(v, x);
                    sw1.Stop();
                    naiveTotalTime += sw1.Elapsed.TotalMilliseconds;
                }

                if (runHorner)
                {
                    Stopwatch sw2 = Stopwatch.StartNew();
                    double resultHorner = AlgorithmsBenchmark.HornerPolynomial(v, x);
                    sw2.Stop();
                    hornerTotalTime += sw2.Elapsed.TotalMilliseconds;
                }

                if (runBubble)
                {
                    double[] vCopy1 = (double[])v.Clone();
                    Stopwatch sw3 = Stopwatch.StartNew();
                    AlgorithmsBenchmark.BubbleSort(vCopy1);
                    sw3.Stop();
                    bubbleTotalTime += sw3.Elapsed.TotalMilliseconds;
                }

                if (runShell)
                {
                    double[] vCopy2 = (double[])v.Clone();
                    Stopwatch sw4 = Stopwatch.StartNew();
                    AlgorithmsBenchmark.ShellSort(vCopy2);
                    sw4.Stop();
                    shellTotalTime += sw4.Elapsed.TotalMilliseconds;
                }

                if (runMatrix)
                {
                    int[,] matrixA = new int[n, n];
                    int[,] matrixB = new int[n, n];
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            matrixA[i, j] = rand.Next(1, 100);
                            matrixB[i, j] = rand.Next(1, 100);
                        }
                    }

                    Stopwatch sw5 = Stopwatch.StartNew();
                    AlgorithmsBenchmark.MultiplyMatrix(matrixA, matrixB);
                    sw5.Stop();
                    matrixTotalTime += sw5.Elapsed.TotalMilliseconds;
                }

                if (runPow || runRecPow || runQuickPow || runClassicalQuickPow)
                {
                    long baseVal = rand.Next(2, 10);
                    int exponent = n;

                    if (runPow)
                    {
                        Stopwatch sw6 = Stopwatch.StartNew();
                        AlgorithmsBenchmark.Pow(baseVal, exponent);
                        sw6.Stop();
                        powTotalTime += sw6.Elapsed.TotalMilliseconds;
                    }

                    if (runRecPow)
                    {
                        Stopwatch sw7 = Stopwatch.StartNew();
                        AlgorithmsBenchmark.RecPow(baseVal, exponent);
                        sw7.Stop();
                        recPowTotalTime += sw7.Elapsed.TotalMilliseconds;
                    }

                    if (runQuickPow)
                    {
                        Stopwatch sw8 = Stopwatch.StartNew();
                        AlgorithmsBenchmark.QuickPow(baseVal, exponent);
                        sw8.Stop();
                        quickPowTotalTime += sw8.Elapsed.TotalMilliseconds;
                    }

                    if (runClassicalQuickPow)
                    {
                        Stopwatch sw9 = Stopwatch.StartNew();
                        AlgorithmsBenchmark.ClassicalQuickPow(baseVal, exponent);
                        sw9.Stop();
                        classicalQuickPowTotalTime += sw9.Elapsed.TotalMilliseconds;
                    }
                }
            }

            nValues.Add(n);
            naiveTimes.Add(runNaive ? naiveTotalTime / repeats : 0);
            hornerTimes.Add(runHorner ? hornerTotalTime / repeats : 0);
            bubbleTimes.Add(runBubble ? bubbleTotalTime / repeats : 0);
            shellTimes.Add(runShell ? shellTotalTime / repeats : 0);
            matrixTimes.Add(runMatrix ? matrixTotalTime / repeats : 0);
            powTimes.Add(runPow ? powTotalTime / repeats : 0);
            recPowTimes.Add(runRecPow ? recPowTotalTime / repeats : 0);
            quickPowTimes.Add(runQuickPow ? quickPowTotalTime / repeats : 0);
            classicalQuickPowTimes.Add(runClassicalQuickPow ? classicalQuickPowTotalTime / repeats : 0);

            if (n % 100 == 0 || (n % 10 == 0 && maxN <= 100))
                Console.WriteLine($"Обработано n = {n}");
        }

        SaveDataToCSV(nValues, naiveTimes, hornerTimes, bubbleTimes, shellTimes,
                     matrixTimes, powTimes, recPowTimes, quickPowTimes, classicalQuickPowTimes);

        CreateSmoothedChart(nValues, naiveTimes, hornerTimes, bubbleTimes, shellTimes,
                           matrixTimes, powTimes, recPowTimes, quickPowTimes, classicalQuickPowTimes,
                           runNaive, runHorner, runBubble, runShell, runMatrix,
                           runPow, runRecPow, runQuickPow, runClassicalQuickPow);

        Console.WriteLine("\nАнализ завершен. Результаты сохранены в файлы:");
        Console.WriteLine("- results.csv - данные измерений");
        Console.WriteLine("- chart_comparison.png - сглаженный график сравнения алгоритмов");

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void SaveDataToCSV(List<int> nValues, List<double> naiveTimes, List<double> hornerTimes,
                             List<double> bubbleTimes, List<double> shellTimes, List<double> matrixTimes,
                             List<double> powTimes, List<double> recPowTimes, List<double> quickPowTimes,
                             List<double> classicalQuickPowTimes)
    {
        using (StreamWriter writer = new StreamWriter("results.csv"))
        {
            writer.WriteLine("n,NaiveTime,HornerTime,BubbleTime,ShellTime,MatrixTime,PowTime,RecPowTime,QuickPowTime,ClassicalQuickPowTime");
            for (int i = 0; i < nValues.Count; i++)
            {
                writer.WriteLine($"{nValues[i]},{naiveTimes[i]},{hornerTimes[i]},{bubbleTimes[i]},{shellTimes[i]}," +
                                $"{matrixTimes[i]},{powTimes[i]},{recPowTimes[i]},{quickPowTimes[i]},{classicalQuickPowTimes[i]}");
            }
        }
    }

    static void CreateSmoothedChart(List<int> nValues, List<double> naiveTimes, List<double> hornerTimes,
                                   List<double> bubbleTimes, List<double> shellTimes, List<double> matrixTimes,
                                   List<double> powTimes, List<double> recPowTimes, List<double> quickPowTimes,
                                   List<double> classicalQuickPowTimes, bool runNaive, bool runHorner,
                                   bool runBubble, bool runShell, bool runMatrix, bool runPow, bool runRecPow,
                                   bool runQuickPow, bool runClassicalQuickPow)
    {
        double[] nArray = nValues.Select(Convert.ToDouble).ToArray();
        double[] naiveArray = naiveTimes.ToArray();
        double[] hornerArray = hornerTimes.ToArray();
        double[] bubbleArray = bubbleTimes.ToArray();
        double[] shellArray = shellTimes.ToArray();
        double[] matrixArray = matrixTimes.ToArray();
        double[] powArray = powTimes.ToArray();
        double[] recPowArray = recPowTimes.ToArray();
        double[] quickPowArray = quickPowTimes.ToArray();
        double[] classicalQuickPowArray = classicalQuickPowTimes.ToArray();

        int windowSize = Math.Max(1, nValues.Count / 50);
        double[] naiveSmoothed = MovingAverage(naiveArray, windowSize);
        double[] hornerSmoothed = MovingAverage(hornerArray, windowSize);
        double[] bubbleSmoothed = MovingAverage(bubbleArray, windowSize);
        double[] shellSmoothed = MovingAverage(shellArray, windowSize);
        double[] matrixSmoothed = MovingAverage(matrixArray, windowSize);
        double[] powSmoothed = MovingAverage(powArray, windowSize);
        double[] recPowSmoothed = MovingAverage(recPowArray, windowSize);
        double[] quickPowSmoothed = MovingAverage(quickPowArray, windowSize);
        double[] classicalQuickPowSmoothed = MovingAverage(classicalQuickPowArray, windowSize);

        Plot plt = new Plot();

        if (runNaive)
        {
            var scatterNaive = plt.Add.Scatter(nArray, naiveSmoothed);
            scatterNaive.LegendText = "Наивный метод";
            scatterNaive.Color = Colors.Blue;
            scatterNaive.LineWidth = 2;
        }

        if (runHorner)
        {
            var scatterHorner = plt.Add.Scatter(nArray, hornerSmoothed);
            scatterHorner.LegendText = "Метод Горнера";
            scatterHorner.Color = Colors.Green;
            scatterHorner.LineWidth = 2;
        }

        if (runBubble)
        {
            var scatterBubble = plt.Add.Scatter(nArray, bubbleSmoothed);
            scatterBubble.LegendText = "Сортировка пузырьком";
            scatterBubble.Color = Colors.Red;
            scatterBubble.LineWidth = 2;
        }

        if (runShell)
        {
            var scatterShell = plt.Add.Scatter(nArray, shellSmoothed);
            scatterShell.LegendText = "Сортировка Шелла";
            scatterShell.Color = Colors.Orange;
            scatterShell.LineWidth = 2;
        }

        if (runMatrix)
        {
            var scatterMatrix = plt.Add.Scatter(nArray, matrixSmoothed);
            scatterMatrix.LegendText = "Умножение матриц";
            scatterMatrix.Color = Colors.Purple;
            scatterMatrix.LineWidth = 2;
        }

        if (runPow)
        {
            var scatterPow = plt.Add.Scatter(nArray, powSmoothed);
            scatterPow.LegendText = "Возведение в степень (итеративное)";
            scatterPow.Color = Colors.Brown;
            scatterPow.LineWidth = 2;
        }

        if (runRecPow)
        {
            var scatterRecPow = plt.Add.Scatter(nArray, recPowSmoothed);
            scatterRecPow.LegendText = "Возведение в степень (рекурсивное)";
            scatterRecPow.Color = Colors.Pink;
            scatterRecPow.LineWidth = 2;
        }

        if (runQuickPow)
        {
            var scatterQuickPow = plt.Add.Scatter(nArray, quickPowSmoothed);
            scatterQuickPow.LegendText = "Быстрое возведение в степень";
            scatterQuickPow.Color = Colors.Gray;
            scatterQuickPow.LineWidth = 2;
        }

        if (runClassicalQuickPow)
        {
            var scatterClassicalQuickPow = plt.Add.Scatter(nArray, classicalQuickPowSmoothed);
            scatterClassicalQuickPow.LegendText = "Классическое быстрое возведение в степень";
            scatterClassicalQuickPow.Color = Colors.Cyan;
            scatterClassicalQuickPow.LineWidth = 2;
        }

        if (runNaive || runHorner || runBubble || runShell || runMatrix ||
            runPow || runRecPow || runQuickPow || runClassicalQuickPow)
        {
            plt.Title("Сравнение времени выполнения алгоритмов");
            plt.Axes.Bottom.Label.Text = "Размерность n";
            plt.Axes.Left.Label.Text = "Время (мс)";
            plt.ShowLegend();
            plt.SavePng("chart_comparison.png", 1200, 800);
        }
    }

    // Метод для сглаживания данных
    static double[] MovingAverage(double[] data, int windowSize)
    {
        double[] smoothed = new double[data.Length];
        int halfWindow = windowSize / 2;

        for (int i = 0; i < data.Length; i++)
        {
            int start = Math.Max(0, i - halfWindow);
            int end = Math.Min(data.Length - 1, i + halfWindow);
            double sum = 0;
            int count = 0;

            for (int j = start; j <= end; j++)
            {
                sum += data[j];
                count++;
            }

            smoothed[i] = sum / count;
        }

        return smoothed;
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

public class AlgorithmsBenchmark
{
    // 4
    public static double NaivePolynomial(double[] coefficients, double x)
    {
        double result = 0;
        for (int i = 0; i < coefficients.Length; i++)
        {
            result += coefficients[i] * Math.Pow(x, i);
        }
        return result;
    }
    // 4
    public static double HornerPolynomial(double[] coefficients, double x)
    {
        double result = coefficients[coefficients.Length - 1];
        for (int i = coefficients.Length - 2; i >= 0; i--)
        {
            result = coefficients[i] + x * result;
        }
        return result;
    }

    // 5
    public static void BubbleSort(double[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    double temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    // Сортировка Шелла
    public static void ShellSort(double[] arr)
    {
        int n = arr.Length;

        // Вычисляем начальный промежуток по последовательности Кнута
        int gap = 1;
        while (gap < n / 3)
        {
            gap = 3 * gap + 1;
        }

        // Последовательно уменьшаем промежуток
        while (gap >= 1)
        {
            // Выполняем сортировку вставками для этого промежутка
            for (int i = gap; i < n; i++)
            {
                double temp = arr[i];
                int j;
                for (j = i; j >= gap && arr[j - gap] > temp; j -= gap)
                {
                    arr[j] = arr[j - gap];
                }
                arr[j] = temp;
            }
            gap /= 3;
        }
    }

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