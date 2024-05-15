package com.company;

// Клас, що реалізує потік виробника
public class Producer implements Runnable {
    private final ProducerConsumerExample example;

    Producer(ProducerConsumerExample example) {
        this.example = example;
    }

    @Override
    public void run() {
        int item = 0;
        while (true) {
            try {
                example.produce(item++); // Викликаємо метод produce для додавання продукції
                Thread.sleep((int) (Math.random() * 1000)); // Симулюємо час виробництва
                if (example.producedCount >= example.totalProduction) {
                    break; // Завершуємо потік, якщо вироблено достатньо продукції
                }
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt(); // Встановлюємо стан переривання
                break;
            }
        }
    }
}
