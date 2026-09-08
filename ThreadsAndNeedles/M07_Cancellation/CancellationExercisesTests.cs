using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class CancellationExercisesTests
{
    [Fact]
    public async Task RepeatUntilCancelledAsyncStopsWhenCancelled()
    {
        using var source = new CancellationTokenSource();
        var calls = 0;

        async Task Action()
        {
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
}
