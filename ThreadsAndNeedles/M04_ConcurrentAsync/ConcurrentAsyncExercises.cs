namespace ConcurrencyLab;

public static class ConcurrentAsyncExercises
{
    public static async Task<(string First, string Second)> LoadBothAsync(
        Func<Task<string>> firstLoader,
        Func<Task<string>> secondLoader)
    {
        // TODO: start both independent operations before awaiting their results.
        var first = await firstLoader();
        var second = await secondLoader();
        return (first, second);
    }
}
