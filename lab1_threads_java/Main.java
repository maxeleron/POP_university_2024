package com.company;

public class Main {
    public static void main(String[] args) {
        BreakThread breakThread = new BreakThread();

        // Запускаємо задану кількість потоків MainThread
        startThreads(breakThread, 20);

        // Запускаємо BreakThread в окремому потоці
        new Thread(breakThread).start();
    }

    private static void startThreads(BreakThread breakThread, int threadCount) {
        for (int i = 1; i <= threadCount; i++) {
            new MainThread(i, breakThread).start();
        }
    }
}
