package com.company;

public class BreakThread implements Runnable{
    private volatile boolean canBreak = false;

    @Override
    public void run() {
        try {
            Thread.sleep(25 * 1000); // Запускаємо таймер на 25 секунд
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        canBreak = true;
    }

    public boolean isCanBreak() {
        return canBreak;
    }
}
