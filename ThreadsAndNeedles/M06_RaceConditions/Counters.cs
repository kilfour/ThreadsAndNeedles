namespace ThreadsAndNeedles.M06_RaceConditions;

public sealed class UnsafeCounter
{
    private int value;

    public int Value => value;

    public void Increment()
    {
        value++;
    }
}

public sealed class SafeCounter
{
    private int value;

    // TODO: make reads safe while other threads may be writing.
    public int Value => value;

    public void Increment()
    {
        // TODO: make increment atomic.
        value++;
    }
}

public sealed class SafeLedger
{
    private readonly object gate = new();
    private decimal balance;
    private int operationCount;

    // TODO: take the snapshot under the same lock used by Add.
    public (decimal Balance, int OperationCount) Snapshot()
        => (balance, operationCount);

    public void Add(decimal amount)
    {
        // TODO: update both values as one critical section.
        balance += amount;
        operationCount++;
    }
}
