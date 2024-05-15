package com.company;

import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.Semaphore;
import java.util.Scanner;

public class ProducerConsumerExample {
    // Сховище з обмеженою місткістю
    private final Queue<Integer> queue = new LinkedList<>();

    // Семафори для контролю доступу до сховища
    private Semaphore items; // Кількість доступних для споживачів елементів
    private Semaphore spaces; // Кількість вільних місць для виробників
    private final Semaphore mutex = new Semaphore(1); // М'ютекс для взаємного виключення

    // Загальна кількість продукції, яку потрібно обробити
    public int totalProduction;

    // Лічильники виробленої та спожитої продукції
    public volatile int producedCount = 0;
    public volatile int consumedCount = 0;

    // Конструктор для ініціалізації семафорів та параметрів сховища
    public ProducerConsumerExample(int capacity, int totalProduction) {
        this.items = new Semaphore(0);
        this.spaces = new Semaphore(capacity);
        this.totalProduction = totalProduction;
    }

    public void Main(Semaphore items, Semaphore spaces) {
        this.items = items;
        this.spaces = spaces;
        totalProduction = 0;
    }

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        // Введення користувачем параметрів програми
        System.out.print("Введіть об'єм сховища: ");
        int capacity = scanner.nextInt();
        System.out.print("Введіть кількість виробників: ");
        int numProducers = scanner.nextInt();
        System.out.print("Введіть кількість споживачів: ");
        int numConsumers = scanner.nextInt();
        System.out.print("Введіть кількість продукції, яка буде оброблена: ");
        int totalProduction = scanner.nextInt();

        // Створення екземпляра ProducerConsumerExample з заданими параметрами
        ProducerConsumerExample example = new ProducerConsumerExample(capacity, totalProduction);

        // Створення та запуск потоків виробників
        for (int i = 0; i < numProducers; i++) {
            new Thread(new Producer(example)).start();
        }

        // Створення та запуск потоків споживачів
        for (int i = 0; i < numConsumers; i++) {
            new Thread(new Consumer(example)).start();
        }
    }

    // Метод для додавання продукції у сховище
    public void produce(int item) throws InterruptedException {
        if (producedCount >= totalProduction) return; // Перевірка, чи досягнуто максимальну кількість продукції
        spaces.acquire(); // Очікуємо на наявність вільного місця
        mutex.acquire();  // Входимо в критичну секцію
        if (producedCount < totalProduction) { // Додатково перевіряємо умову в критичній секції
            queue.offer(item);
            producedCount++;
            System.out.println("Produced: " + item + " (Total Produced: " + producedCount + ")");
        }
        mutex.release(); // Виходимо з критичної секції
        items.release(); // Збільшуємо кількість доступних елементів
    }

    // Метод для використання продукції зі сховища
    public int consume() throws InterruptedException {
        items.acquire(); // Очікуємо на наявність доступних елементів
        mutex.acquire(); // Входимо в критичну секцію
        int item = -1;
        if (consumedCount < totalProduction) { // Перевіряємо умову споживання в критичній секції
            item = queue.poll();
            consumedCount++;
            System.out.println("Consumed: " + item + " (Total Consumed: " + consumedCount + ")");
        }
        mutex.release(); // Виходимо з критичної секції
        spaces.release(); // Збільшуємо кількість вільних місць
        return item;
    }
}
