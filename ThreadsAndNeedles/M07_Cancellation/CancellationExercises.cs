namespace ThreadsAndNeedles.M07_Cancellation;

public static class CancellationExercises
{
    public static async Task RepeatUntilCancelledAsync(
        Func<CancellationToken, Task> action,
        TimeSpan delay,
        CancellationToken cancellationToken)
    {
        // TODO: repeatedly execute action, forward the token and observe cancellation.
        await action(CancellationToken.None);
    }
}
