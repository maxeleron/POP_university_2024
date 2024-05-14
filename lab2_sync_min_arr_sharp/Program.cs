using System;
using System.Threading;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            long startTime = DateTime.Now.Ticks;
            int arrayLength = 500000000; // Довжина масиву 0,5млрд.
            int threadCount = 32; // Кількість потоків
            int[] array = GenerateArray(arrayLength); // Генерація масиву

            // Створення та запуск потоків
            MinFinderThread[] threads = new MinFinderThread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new MinFinderThread(i * (arrayLength / threadCount),
                        (i + 1) * (arrayLength / threadCount), array);
                new Thread(threads[i].Run).Start();
            }

            // Очікування завершення роботи потоків
            foreach (MinFinderThread thread in threads)
            {
                thread.WaitForCompletion();
            }

            // Пошук мінімального значення серед усіх мінімумів знайдених потоками
            int globalMinIndex = -1;
            int globalMinValue = int.MaxValue;
            foreach (MinFinderThread thread in threads)
            {
                if (thread.GetMinValue() < globalMinValue)
                {
                    globalMinValue = thread.GetMinValue();
                    globalMinIndex = thread.GetMinIndex();
                }
            }

            // Підрахунок занятого часу виконання програми в наносекундах та конвертування в мілісекунди
            long endTime = DateTime.Now.Ticks;
            long elapsedTime = (endTime - startTime) / TimeSpan.TicksPerMillisecond; // Конвертуємо час виконання в мілісекунди

            // Print results
            Console.WriteLine( "Час виконання в мілісекундах: " + elapsedTime );
            Console.WriteLine( "Значення мінімального елементу: " + globalMinValue );
            Console.WriteLine( "Індекс мінімального елементу: " + globalMinIndex );
            Console.ReadKey();
        }

        private static int[] GenerateArray(int length)
        {
            int[] array = new int[length];
            Random random = new Random();
            // Заповнюємо масив згенерованими позитивними числами
            for (int i = 0; i < length; i++)
            {
                array[i] = random.Next(100); // Випадкові додатні значення від 0 до 99
            }

            // Генеруємо рандомний індекс в масиві
            int randomIndex = random.Next(array.Length);
            // Записуємо від'ємне значення від -1 до -99 до випадкового елемента
            array[randomIndex] = -random.Next(100);

            return array;
        }
    }

    class MinFinderThread
    {
        private readonly int startIndex;
        private readonly int endIndex;
        private readonly int[] array;
        private int minValue = int.MaxValue;
        private int minIndex = -1;
        private bool isCompleted = false;

        public MinFinderThread(int startIndex, int endIndex, int[] array)
        {
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            this.array = array;
        }

        public void Run()
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                if (array[i] < minValue)
                {
                    minValue = array[i];
                    minIndex = i;
                }
            }
            isCompleted = true;
        }

        public void WaitForCompletion()
        {
            while (!isCompleted) Thread.Sleep(1);
        }

        public int GetMinValue()
        {
            return minValue;
        }

        public int GetMinIndex()
        {
            return minIndex;
        }
    }
}
