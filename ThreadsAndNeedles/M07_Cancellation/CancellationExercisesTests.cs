namespace ThreadsAndNeedles.M07_Cancellation;

public class CancellationExercisesTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task RepeatUntilCancelledAsyncStopsWhenCancelled()
    {
        using var source = new CancellationTokenSource();
        var calls = 0;

        async Task Action(CancellationToken token)
        {
            Assert.Equal(source.Token, token);
            calls++;

            if (calls == 3)
            {
                await source.CancelAsync();
            }
        }

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            CancellationExercises.RepeatUntilCancelledAsync(
                Action,
                TimeSpan.FromMilliseconds(5),
                source.Token));

        Assert.Equal(3, calls);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task RepeatUntilCancelledAsyncDoesNoWorkWhenAlreadyCancelled()
    {
        using var source = new CancellationTokenSource();
        await source.CancelAsync();
        var calls = 0;

        Task Action(CancellationToken token)
        {
            calls++;
            return Task.CompletedTask;
        }

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            CancellationExercises.RepeatUntilCancelledAsync(
                Action,
                TimeSpan.Zero,
                source.Token));

        Assert.Equal(0, calls);
    }
}
