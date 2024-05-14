package com.company;

public class Main {

    public static void main(String[] args) {
        long startTime = System.nanoTime();
        int arrayLength = 1000_000_000; // Довжина масиву 1млрд.
        int threadCount = 32; // Кількість потоків
        int[] array = generateArray(arrayLength); // Генерація масиву
    }

}
