using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class CpuWorkTests
{
    [Fact]
    public async Task CalculateOnThreadPoolAsyncRunsCalculationAwayFromCaller()
    {
        const int iterations = 100_000;
        var callerThreadId = Environment.CurrentManagedThreadId;
        var expected = CpuWork.Calculate(iterations);

        var actual = await CpuWork.CalculateOnThreadPoolAsync(iterations);

        Assert.Equal(expected, actual.Result);
        Assert.NotEqual(callerThreadId, actual.ThreadId);
    }
}
