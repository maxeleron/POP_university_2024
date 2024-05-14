package com.company;

// Клас для потоку, який знаходить мінімальний елемент у своїй частині масиву
class MinFinderThread extends Thread {
    private final int startIndex;
    private final int endIndex;
    private final int[] array;
    private int minValue = Integer.MAX_VALUE;
    private int minIndex = -1;

    public MinFinderThread(int startIndex, int endIndex, int[] array) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.array = array;
    }

    @Override
    public void run() {
        for (int i = startIndex; i < endIndex; i++) {
            if (array[i] < minValue) {
                minValue = array[i];
                minIndex = i;
            }
        }
    }

    // Методи доступу до результатів потоку
    public int getMinValue() {
        return minValue;
    }

    public int getMinIndex() {
        return minIndex;
    }
}
