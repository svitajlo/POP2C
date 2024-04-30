using System;
using System.Threading;

namespace ThreadMinValueSharp
{
    class Program
    {
        private static readonly int dim = 10000000;
        private static readonly int threadNum = 5; // Задана кількість потоків

        private readonly Thread[] threads = new Thread[threadNum];
        private readonly int[] arr = new int[dim];
        private int minValue = int.MaxValue;
        private int minIndex = -1;

        static void Main(string[] args)
        {
            Program main = new Program();
            main.InitArr(); // Генерація масиву
            main.ParallelMin(); // Пошук мінімального значення
            Console.WriteLine($"Min value: {main.minValue}, Index: {main.minIndex}");
            Console.ReadKey();
        }

        private void ParallelMin()
        {
            int chunkSize = dim / threadNum; // Розмір частини масиву для кожного потоку
            for (int i = 0; i < threadNum; i++)
            {
                int startIndex = i * chunkSize;
                int finishIndex = (i + 1) * chunkSize;
                threads[i] = new Thread(() => FindMin(startIndex, finishIndex));
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void FindMin(int startIndex, int finishIndex)
        {
            int localMinValue = int.MaxValue;
            int localMinIndex = -1;
            for (int i = startIndex; i < finishIndex; i++)
            {
                if (arr[i] < localMinValue)
                {
                    localMinValue = arr[i];
                    localMinIndex = i;
                }
            }
            lock (this)
            {
                if (localMinValue < minValue)
                {
                    minValue = localMinValue;
                    minIndex = localMinIndex;
                }
            }
        }

        private void InitArr()
        {
            Random rnd = new Random();
            for (int i = 0; i < dim; i++)
            {
                arr[i] = rnd.Next(int.MinValue, 0); // Замінюємо генерацію на від'ємні числа
            }
        }
    }
}
