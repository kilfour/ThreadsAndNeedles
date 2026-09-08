using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class CpuWorkTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task CalculateOnThreadPoolAsyncRunsCalculationOnThreadPool()
    {
        const int iterations = 100_000;
        var expected = CpuWork.Calculate(iterations);
        var invocation = new TaskCompletionSource<(
            int CallerThreadId,
            Task<(long Result, int ThreadId, bool IsThreadPoolThread)> Work)>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        var caller = new Thread(() =>
        {
            try
            {
                var callerThreadId = Environment.CurrentManagedThreadId;
                var work = CpuWork.CalculateOnThreadPoolAsync(iterations);
                invocation.SetResult((callerThreadId, work));
            }
            catch (Exception exception)
            {
                invocation.SetException(exception);
            }
        });

        caller.Start();

        var scheduled = await invocation.Task.WaitAsync(TimeSpan.FromSeconds(2));
        Assert.True(caller.Join(TimeSpan.FromSeconds(2)), "The dedicated caller thread did not finish.");

        var actual = await scheduled.Work;

        Assert.Equal(expected, actual.Result);
        Assert.True(actual.IsThreadPoolThread);
        Assert.NotEqual(scheduled.CallerThreadId, actual.ThreadId);
    }
}
