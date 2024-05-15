package com.company;

// Клас, що реалізує потік споживача
public class Consumer implements Runnable {
    private final ProducerConsumerExample example;

    Consumer(ProducerConsumerExample example) {
        this.example = example;
    }

    @Override
    public void run() {
        while (true) {
            try {
                int item = example.consume(); // Викликаємо метод consume для споживання продукції
                Thread.sleep((int) (Math.random() * 1000)); // Симулюємо час споживання
                if (example.consumedCount >= example.totalProduction) {
                    break; // Завершуємо потік, якщо спожито достатньо продукції
                }
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt(); // Встановлюємо стан переривання
                break;
            }
        }
    }
}
