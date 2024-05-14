package com.company;
import java.util.Random;

public class Main {

    public static void main(String[] args) {
        long startTime = System.nanoTime();
        int arrayLength = 1000_000_000; // Довжина масиву 1млрд.
        int threadCount = 32; // Кількість потоків
        int[] array = generateArray(arrayLength); // Генерація масиву

        // Створення та запуск потоків
        MinFinderThread[] threads = new MinFinderThread[threadCount];
        for (int i = 0; i < threadCount; i++) {
            threads[i] = new MinFinderThread(i * (arrayLength / threadCount), (i + 1) * (arrayLength / threadCount), array);
            threads[i].start();
        }

        // Очікування завершення роботи потоків
        try {
            for (MinFinderThread thread : threads) {
                thread.join();
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        // Пошук мінімального значення серед усіх мінімумів знайдених потоками
        int globalMinIndex = -1;
        int globalMinValue = Integer.MAX_VALUE;
        for (MinFinderThread thread : threads) {
            if (thread.getMinValue() < globalMinValue) {
                globalMinValue = thread.getMinValue();
                globalMinIndex = thread.getMinIndex();
            }
        }

        // Підрахунок занятого часу виконання програми в наносекундах та конвертування в мілісекунди
        long endTime = System.nanoTime();
        long elapsedTime = (endTime - startTime)/1000_000; // Конвертуємо час виконання в мілісекунди


        // Виведення результатів
        System.out.println( "Час виконання в мілісекундах: " + elapsedTime );
        System.out.println( "Значення мінімального елементу: " + globalMinValue );
        System.out.println( "Індекс мінімального елементу: " + globalMinIndex );
    }

    private static int[] generateArray(int length) {
        int[] array = new int[length];
        Random random = new Random();
        for (int i = 0; i < length; i++) {
            if (random.nextBoolean()) { // Заповнюємо масив згенерованими позитивними числами
                array[i] = random.nextInt(100); // Випадкові додатні значення від 0 до 99
            }
        }

        // Генеруємо рандомний індекс в масиві
        int randomIndex = random.nextInt(array.length);

        array[randomIndex] = -random.nextInt(100); // Записуємо від'ємне значення від -1 до -99 до випадкового елемента

        return array;
    }

}
