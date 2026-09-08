namespace ConcurrencyLab;

public static class CancellationExercises
{
    public static async Task RepeatUntilCancelledAsync(
        Func<Task> action,
        TimeSpan delay,
        CancellationToken cancellationToken)
    {
        // TODO: repeatedly execute action and observe cancellation.
        await action();
    }
}
