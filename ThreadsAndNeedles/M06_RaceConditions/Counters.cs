namespace ConcurrencyLab;

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

    public int Value => value;

    public void Increment()
    {
        // TODO: make increment atomic.
        value++;
    }
}

public sealed class SafeLedger
{
    private decimal balance;
    private int operationCount;

    public (decimal Balance, int OperationCount) Snapshot()
        => (balance, operationCount);

    public void Add(decimal amount)
    {
        // TODO: update both values as one critical section.
        balance += amount;
        operationCount++;
    }
}
