using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab3_semaphore_sharp
{
    public class ProducerConsumerExample
    {
        class Program
        {           
            public static void Main(string[] args)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.Write("Введіть об'єм сховища: ");
                int capacity = int.Parse(Console.ReadLine());
                Console.Write("Введіть кількість виробників: ");
                int numProducers = int.Parse(Console.ReadLine());
                Console.Write("Введіть кількість споживачів: ");
                int numConsumers = int.Parse(Console.ReadLine());
                Console.Write("Введіть кількість продукції, яка буде оброблена: ");
                int totalProduction = int.Parse(Console.ReadLine());

                // Create an instance of ProducerConsumerExample with the given parameters
                ProducerConsumerExample example = new ProducerConsumerExample(capacity, totalProduction);

                // Create and start producer threads
                for (int i = 0; i < numProducers; i++)
                {
                    new Thread(new Producer(example).Run).Start();
                }

                // Create and start consumer threads
                for (int i = 0; i < numConsumers; i++)
                {
                    new Thread(new Consumer(example).Run).Start();
                }
            }
        }

        private Queue<int> queue = new Queue<int>();

        // Семафори для контролю доступу до сховища
        private Semaphore items; // Кількість доступних для споживачів елементів
        private Semaphore spaces; // Кількість вільних місць для виробників
        private Semaphore mutex = new Semaphore(1, 1); // М'ютекс для взаємного виключення

        // Загальна кількість продукції, яку потрібно обробити
        public int totalProduction;

        // Лічильники виробленої та спожитої продукції
        public volatile int producedCount = 0;
        public volatile int consumedCount = 0;

        // Конструктор для ініціалізації семафорів та параметрів сховища
        public ProducerConsumerExample(int capacity, int totalProduction)
        {
            this.items = new Semaphore(0, int.MaxValue);
            this.spaces = new Semaphore(capacity, int.MaxValue);
            this.totalProduction = totalProduction;
        }

        // Метод для додавання продукції у сховище
        public void Produce(int item)
        {
            if (producedCount >= totalProduction) return; // Перевірка, чи досягнуто максимальну кількість продукції
            spaces.WaitOne(); // Очікуємо на наявність вільного місця
            mutex.WaitOne(); // Входимо в критичну секцію
            if (producedCount < totalProduction)
            { // Додатково перевіряємо умову в критичній секції
                queue.Enqueue(item);
                producedCount++;
                Console.WriteLine("Виготовлено: " + item + " (Всього виготовлено: " + producedCount + ")");
            }
            mutex.Release(); // Виходимо з критичної секції
            items.Release(); // Збільшуємо кількість доступних елементів
        }

        // Метод для використання продукції зі сховища
        public int Consume()
        {
            items.WaitOne(); // Очікуємо на наявність доступних елементів
            mutex.WaitOne(); // Входимо в критичну секцію
            int item = -1;
            if (consumedCount < totalProduction)
            { // Перевіряємо умову споживання в критичній секції
                item = queue.Dequeue();
                consumedCount++;
                Console.WriteLine("Спожито: " + item + " (Всього спожито: " + consumedCount + ")");
            }
            mutex.Release(); // Виходимо з критичної секції
            spaces.Release(); // Збільшуємо кількість вільних місць
            return item;
        }
    }

    // Клас, що реалізує потік виробника
    public class Producer : Runnable
    {
        private readonly ProducerConsumerExample example;

        public Producer(ProducerConsumerExample example)
        {
            this.example = example;
        }

        public override void Run()
        {
            int item = 0;
            while (true)
            {
                try
                {
                    example.Produce(item++); // Викликаємо метод produce для додавання продукції
                    Thread.Sleep((int)(new Random().NextDouble() * 1000)); // Симулюємо час виробництва
                    if (example.producedCount >= example.totalProduction)
                    {
                        break; // Завершуємо потік, якщо вироблено достатньо продукції
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Thread.CurrentThread.Interrupt(); // Встановлюємо стан переривання
                    break;
                }
            }
        }
    }

    // Клас, що реалізує потік споживача
    public class Consumer : Runnable
    {
        private readonly ProducerConsumerExample example;

        public Consumer(ProducerConsumerExample example)
        {
            this.example = example;
        }

        public override void Run()
        {
            while (true)
            {
                try
                {
                    int item = example.Consume(); // Викликаємо метод consume для споживання продукції
                    Thread.Sleep((int)(new Random().NextDouble() * 1000)); // Симулюємо час споживання
                    if (example.consumedCount >= example.totalProduction)
                    {
                        break; // Завершуємо потік, якщо спожито достатньо продукції
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Thread.CurrentThread.Interrupt(); // Встановлюємо стан переривання
                    break;
                }
            }
        }
    }
}

public abstract class Runnable
{
    public abstract void Run();
}
