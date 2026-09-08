namespace ThreadsAndNeedles.M06_RaceConditions;

public class CounterTests
{
    [Fact(Skip = "Demonstration only: remove Skip locally and run repeatedly.")]
    public void UnsafeCounterExperimentExpectedTotal()
    {
        var counter = new UnsafeCounter();

        Parallel.For(0, 1_000_000, _ => counter.Increment());

        Assert.Equal(1_000_000, counter.Value);
    }

    [Fact(Skip = "Not Implemented")]
    public void ThreadSafeCounterNeverLosesUpdates()
    {
        var counter = new SafeCounter();

        Parallel.For(0, 100_000, _ => counter.Increment());

        Assert.Equal(100_000, counter.Value);
    }

    [Fact(Skip = "Not Implemented")]
    public void SafeLedgerUpdatesBalanceAndCountTogether()
    {
        var ledger = new SafeLedger();

        Parallel.For(0, 10_000, _ => ledger.Add(1m));

        var snapshot = ledger.Snapshot();

        Assert.Equal(10_000m, snapshot.Balance);
        Assert.Equal(10_000, snapshot.OperationCount);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task SafeLedgerSnapshotsRemainConsistentDuringWrites()
    {
        const int workerCount = 4;
        const int writesPerWorker = 100_000;
        var ledger = new SafeLedger();
        var start = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var writers = Enumerable.Range(0, workerCount)
            .Select(_ => Task.Run(async () =>
            {
                await start.Task;

                for (var i = 0; i < writesPerWorker; i++)
                {
                    ledger.Add(1m);
                }
            }))
            .ToArray();

        var allWriters = Task.WhenAll(writers);
        var reader = Task.Run(async () =>
        {
            await start.Task;

            while (!allWriters.IsCompleted)
            {
                var snapshot = ledger.Snapshot();
                Assert.Equal(snapshot.OperationCount, snapshot.Balance);
            }
        });

        start.SetResult();

        await Task.WhenAll(allWriters, reader);

        var finalSnapshot = ledger.Snapshot();
        Assert.Equal(workerCount * writesPerWorker, finalSnapshot.OperationCount);
        Assert.Equal(finalSnapshot.OperationCount, finalSnapshot.Balance);
    }
}
