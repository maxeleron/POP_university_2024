package com.company;

public class MainThread extends Thread{
    private final int id;
    private final BreakThread breakThread;

    public MainThread(int id, BreakThread breakThread) {
        this.id = id;
        this.breakThread = breakThread;
    }

    @Override
    public void run() {
        long sum = 0;
        // Цикл працює доки ми не змінимо змінну canBreak через заданий інтервал)
        while ( !breakThread.isCanBreak() ) {
            sum++;
        }
        System.out.println( "Thread " + id + " - Total: " + sum );
    }
}
