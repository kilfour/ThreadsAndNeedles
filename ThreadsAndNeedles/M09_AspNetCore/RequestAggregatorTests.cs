using System.Diagnostics;
using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class RequestAggregatorTests
{
    [Fact]
    public async Task LoadAsyncStartsIndependentCallsConcurrently()
    {
        var aggregator = new RequestAggregator();
        var stopwatch = Stopwatch.StartNew();

        async Task<string> Load(string value, CancellationToken token)
        {
            await Task.Delay(140, token);
            return value;
        }

        var result = await aggregator.LoadAsync(
            token => Load("A", token),
            token => Load("B", token),
            CancellationToken.None);

        stopwatch.Stop();

        Assert.Equal(new AggregatedResult("A", "B"), result);
        Assert.True(stopwatch.Elapsed < TimeSpan.FromMilliseconds(240));
    }

    [Fact]
    public async Task LoadAsyncForwardsCancellation()
    {
        var aggregator = new RequestAggregator();
        using var source = new CancellationTokenSource(30);

        async Task<string> Load(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), token);
            return "never";
        }

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            aggregator.LoadAsync(Load, Load, source.Token));
    }
}
