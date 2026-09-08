namespace ConcurrencyLab;

public static class TaskExperiments
{
    public static async Task<string> CompleteLater(string value, TimeSpan delay)
    {
        // TODO: wait asynchronously.
        await Task.CompletedTask;
        return value;
    }
}
