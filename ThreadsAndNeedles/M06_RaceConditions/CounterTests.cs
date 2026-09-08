using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class CounterTests
{
    [Fact]
    public void ThreadSafeCounterNeverLosesUpdates()
    {
        var counter = new SafeCounter();

        Parallel.For(0, 100_000, _ => counter.Increment());

        Assert.Equal(100_000, counter.Value);
    }

    [Fact]
    public void SafeLedgerUpdatesBalanceAndCountTogether()
    {
        var ledger = new SafeLedger();

        Parallel.For(0, 10_000, _ => ledger.Add(1m));

        var snapshot = ledger.Snapshot();

        Assert.Equal(10_000m, snapshot.Balance);
        Assert.Equal(10_000, snapshot.OperationCount);
    }
}
