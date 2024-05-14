using System;
using System.Threading;

namespace Lab1_Threads
{
    class Program
    {
        static void Main(string[] args)
        {
            BreakThread breakThread = new BreakThread();

            // Запускаємо задану кількість потоків MainThread
            StartThreads(breakThread, 20);

            // Запускаємо BreakThread в окремому потоці
            new Thread(new ThreadStart(breakThread.Run)).Start();
        }

        private static void StartThreads(BreakThread breakThread, int threadCount)
        {
            for (int i = 1; i <= threadCount; i++)
            {
                new MainThread(i, breakThread).Start();
            }
        }
    }

    class MainThread
    {
        private readonly int id;
        private readonly BreakThread breakThread;
        private Thread thread;

        public MainThread(int id, BreakThread breakThread)
        {
            this.id = id;
            this.breakThread = breakThread;
            this.thread = new Thread(new ThreadStart(Run));
        }

        public void Start()
        {
            thread.Start();
        }

        public void Run()
        {
            long sum = 0;
            // Цикл працює доки ми не змінимо змінну canBreak через заданий інтервал)
            while ( !breakThread.CanBreak )
            {
                sum++;
            }
            Console.WriteLine( "Thread " + id + " - Total: " + sum );
            Console.ReadKey();
        }
    }

    class BreakThread
    {
        private volatile bool canBreak = false;

        public void Run()
        {
            try
            {
                Thread.Sleep(25 * 1000); // Запускаємо таймер на 25 секунд
            }
            catch (ThreadInterruptedException)
            {
                Thread.CurrentThread.Interrupt();
            }
            canBreak = true;
        }

        public bool CanBreak
        {
            get { return canBreak; }
        }
    }
}
