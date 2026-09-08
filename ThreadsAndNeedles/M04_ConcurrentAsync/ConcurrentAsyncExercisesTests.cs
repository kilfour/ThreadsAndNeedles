using System.Diagnostics;
using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class ConcurrentAsyncExercisesTests
{
    [Fact]
    public async Task LoadBothAsyncStartsBothOperationsBeforeWaiting()
    {
        var stopwatch = Stopwatch.StartNew();

        async Task<string> Load(string value)
        {
            await Task.Delay(150);
            return value;
        }

        var result = await ConcurrentAsyncExercises.LoadBothAsync(
            () => Load("A"),
            () => Load("B"));

        stopwatch.Stop();

        Assert.Equal(("A", "B"), result);
        Assert.True(
            stopwatch.Elapsed < TimeSpan.FromMilliseconds(260),
            $"Expected concurrent execution, elapsed: {stopwatch.ElapsedMilliseconds} ms");
    }
}
