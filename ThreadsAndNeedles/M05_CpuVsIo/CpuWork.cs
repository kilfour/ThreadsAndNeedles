namespace ConcurrencyLab;

public static class CpuWork
{
    public static long Calculate(int iterations)
    {
        long result = 0;

        for (var i = 0; i < iterations; i++)
        {
            result = unchecked(result * 31 + i);
        }

        return result;
    }

    public static async Task<(long Result, int ThreadId, bool IsThreadPoolThread)> CalculateOnThreadPoolAsync(
        int iterations)
    {
        // TODO: execute Calculate on a thread-pool thread and report its thread information.
        await Task.CompletedTask;
        return (
            Calculate(iterations),
            Environment.CurrentManagedThreadId,
            Thread.CurrentThread.IsThreadPoolThread);
    }
}
