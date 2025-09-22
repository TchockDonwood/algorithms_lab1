using System;
using Algorithms_lab1;

namespace Algorithms_lab1
{
    public class Algorithms
    {
        // Алгоритмы для векторов (1-3)

        // 1. f(v) = 1 (постоянная функция)
        public static double ConstFunction(double[] vector)
        {
            double result = 0.0;
            for (int i = 0; i < 1000; i++)
            {
                result += Math.Sin(i * 0.001) + Math.Cos(i * 0.001);
            }
            return 1.0;
        }

        // 2. f(v) = sum от k=1 до n v_k (сумма элементов)
        public static double SumFunction(double[] vector)
        {
            double sum = 0.0;
            for (int i = 0; i < vector.Length; i++)
            {
                sum += vector[i];
            }
            return sum;
        }

        // 3. f(v) = произведение от k=1 до n v_k (произведение элементов)
        public static double ProductFunction(double[] vector)
        {
            double product = 1.0;
            for (int i = 0; i < vector.Length; i++)
            {
                product *= vector[i];
            }
            return product;
        }

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

        // Алгоритм умножения матриц

        public static Matrix MultiplyMatrix(Matrix matrixA, Matrix matrixB)
        {
            var n = matrixA.Rows;
            var m = matrixA.Cols;
            var b = matrixB.Cols;

            var result = new Matrix(n, b);

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

        // Быстрая сортировка
        public static double[] QuickSort(double[] arr, int start, int end)
        {
            if (end <= start)
            {
                return arr;
            }

            else
            {
                double pivot = arr[start];
                int pivotIndex = start;


                for (int i = start + 1; i <= end; i++)
                {
                    if (arr[i] < pivot)
                    {
                        pivotIndex++;
                        double temp = arr[i];
                        arr[i] = arr[pivotIndex];
                        arr[pivotIndex] = temp;
                    }
                }

                // Помещаем опорный элемент на правильную позицию
                double temp2 = arr[start];
                arr[start] = arr[pivotIndex];
                arr[pivotIndex] = temp2;

                QuickSort(arr, start, pivotIndex - 1);
                QuickSort(arr, pivotIndex + 1, end);
                return arr;
            }
        }

        // Сортировка вставками для Timsort
        static void InsertionSort(double[] arr, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                double temp = arr[i];
                int j = i - 1;
                while (j >= left && arr[j] > temp)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = temp;
            }
        }

        // Слияние двух отсортированных подмассивов arr[l..m] и arr[m+1..r]
        const int RUN = 32;

        static void Merge(double[] arr, int l, int m, int r)
        {
            int len1 = m - l + 1, len2 = r - m;
            double[] left = new double[len1];
            double[] right = new double[len2];

            Array.Copy(arr, l, left, 0, len1);
            Array.Copy(arr, m + 1, right, 0, len2);

            int i = 0, j = 0, k = l;

            while (i < len1 && j < len2)
            {
                if (left[i] <= right[j])
                {
                    arr[k++] = left[i++];
                }
                else
                {
                    arr[k++] = right[j++];
                }
            }

            while (i < len1)
                arr[k++] = left[i++];

            while (j < len2)
                arr[k++] = right[j++];
        }

        public static void TimSort(double[] arr)
        {
            int n = arr.Length;

            // Сортируем маленькие куски методом вставок
            for (int i = 0; i < n; i += RUN)
            {
                int right = i + RUN - 1;
                if (right >= n) right = n - 1;
                InsertionSort(arr, i, right);
            }

            // Сливаем отсортированные куски размера RUN, затем 2*RUN, 4*RUN и т.д.
            for (int size = RUN; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = left + 2 * size - 1;
                    if (mid >= n) mid = n - 1;
                    if (right >= n) right = n - 1;

                    if (mid < right)
                    {
                        Merge(arr, left, mid, right);
                    }
                }
            }
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

        // III. Пирамидальная сортировка (HeapSort) - O(n log n)
        public static void HeapSort(int[] arr)
        {
            int n = arr.Length;

            // Построение кучи (перегруппируем массив)
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            // Один за другим извлекаем элементы из кучи
            for (int i = n - 1; i >= 0; i--)
            {
                // Перемещаем текущий корень в конец
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                // вызываем процедуру heapify на уменьшенной куче
                Heapify(arr, i, 0);
            }
        }

        // Процедура для преобразования в двоичную кучу поддерева с корневым узлом i
        private static void Heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int l = 2 * i + 1; // left = 2*i + 1
            int r = 2 * i + 2; // right = 2*i + 2

            // Если левый дочерний элемент больше корня
            if (l < n && arr[l] > arr[largest])
                largest = l;

            // Если правый дочерний элемент больше, чем самый большой элемент на данный момент
            if (r < n && arr[r] > arr[largest])
                largest = r;

            // Если самый большой элемент не корень
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;

                // Рекурсивно преобразуем в двоичную кучу затронутое поддерево
                Heapify(arr, n, largest);
            }
        }

        // Алгоритм поиска всех уникальных подстрок
        public static List<string> GetUniqueSubstrings(string str)
        {
            List<string> uniqueSubstrings = new List<string>();

            // Перебираем все возможные начальные позиции
            for (int start = 0; start < str.Length; start++)
            {
                // Перебираем все возможные длины подстрок
                for (int length = 1; length <= str.Length - start; length++)
                {
                    // Извлекаем подстроку
                    string substring = str.Substring(start, length);

                    // Проверяем, есть ли уже такая подстрока в списке
                    bool isUnique = true;
                    for (int i = 0; i < uniqueSubstrings.Count; i++)
                    {
                        if (uniqueSubstrings[i] == substring)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    // Если подстрока уникальна, добавляем её
                    if (isUnique)
                    {
                        uniqueSubstrings.Add(substring);
                    }
                }
            }

            return uniqueSubstrings;
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

        // Сортировка слиянием
        public static int[] MergeSort(int[] array, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;

                // Рекурсивно сортируем левую и правую половины
                MergeSort(array, left, mid);
                MergeSort(array, mid + 1, right);

                // Сливаем отсортированные половины
                Merge(array, left, mid, right);
            }

            return array;
        }

        public static int[] MergeSort(int[] array)
        {
            return MergeSort(array, 0, array.Length-1);
        }

        // Метод для слияния двух отсортированных массивов
        private static void Merge(int[] array, int left, int mid, int right)
        {
            var leftIndex = left;
            var rightIndex = mid + 1;
            var temp = new int[right - left + 1];
            var index = 0;

            // Сливаем две половины
            while (leftIndex <= mid && rightIndex <= right)
            {
                if (array[leftIndex] < array[rightIndex])
                {
                    temp[index] = array[leftIndex];
                    leftIndex++;
                }
                else
                {
                    temp[index] = array[rightIndex];
                    rightIndex++;
                }
                index++;
            }

            for (var i = leftIndex; i <= mid; i++)
            {
                temp[index] = array[i];
                index++;
            }

            for (var i = rightIndex; i <= right; i++)
            {
                temp[index] = array[i];
                index++;
            }

            for (var i = 0; i < temp.Length; i++)
            {
                array[left + i] = temp[i];
            }
        }
    }
}
