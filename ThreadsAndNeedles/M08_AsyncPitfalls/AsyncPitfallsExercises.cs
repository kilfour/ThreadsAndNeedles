namespace ThreadsAndNeedles.M08_AsyncPitfalls;

public static class AsyncPitfallsExercises
{
    public static Task<string> LoadWithoutBlockingAsync(Func<Task<string>> loader)
    {
        // TODO: do not use Result or Wait.
        return Task.FromResult(loader().Result);
    }

    public static async Task SaveBeforeReturningAsync(Func<Task> save)
    {
        // TODO: make sure save has finished before this method completes.
        _ = save();
        await Task.CompletedTask;
    }

    public static async Task<(string First, string Second)> LoadIndependentValuesAsync(
        Func<Task<string>> first,
        Func<Task<string>> second)
    {
        // TODO: independent operations should not be awaited one after another.
        var firstValue = await first();
        var secondValue = await second();
        return (firstValue, secondValue);
    }
}
