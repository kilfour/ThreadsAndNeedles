namespace ThreadsAndNeedles.M03_AsyncAwait;

public static class AsyncAwaitExercises
{
    public static async Task<string> LoadAndTransformAsync(Func<Task<string>> loader)
    {
        // TODO: call the loader once, await it, trim the result and convert it to uppercase.
        await Task.CompletedTask;
        return string.Empty;
    }
}
