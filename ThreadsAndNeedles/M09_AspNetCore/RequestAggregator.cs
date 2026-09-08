namespace ThreadsAndNeedles.M09_AspNetCore;

public sealed record AggregatedResult(string First, string Second);

public sealed class RequestAggregator
{
    public async Task<AggregatedResult> LoadAsync(
        Func<CancellationToken, Task<string>> first,
        Func<CancellationToken, Task<string>> second,
        CancellationToken cancellationToken)
    {
        // TODO: start both operations concurrently and forward cancellation.
        var firstValue = await first(CancellationToken.None);
        var secondValue = await second(CancellationToken.None);
        return new AggregatedResult(firstValue, secondValue);
    }
}
