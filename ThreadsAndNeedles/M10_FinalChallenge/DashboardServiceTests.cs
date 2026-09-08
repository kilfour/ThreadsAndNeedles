using System.Collections.Concurrent;

namespace ThreadsAndNeedles.M10_FinalChallenge;

public class DashboardServiceTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task LoadAsyncLoadsAllDependenciesConcurrently()
    {
        var gate = new DependencyGate();
        var service = CreateGatedService(gate);

        var loadTask = service.LoadAsync(CancellationToken.None);
        var startedBeforeRelease = gate.StartedCount;

        gate.Release();
        var dashboard = await loadTask;

        Assert.Equal("Ada", dashboard.Profile.Name);
        Assert.Single(dashboard.Orders);
        Assert.Single(dashboard.Recommendations);
        Assert.Equal(1, service.SuccessfulLoads);
        Assert.Equal(3, startedBeforeRelease);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task LoadAsyncForwardsCancellation()
    {
        var gate = new DependencyGate();
        var service = CreateGatedService(gate);
        using var source = new CancellationTokenSource();

        var loadTask = service.LoadAsync(source.Token);
        var startedBeforeCancellation = gate.StartedCount;

        await source.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            loadTask);

        Assert.Equal(3, startedBeforeCancellation);
        Assert.Equal(3, gate.ReceivedTokens.Count);
        Assert.All(gate.ReceivedTokens, token => Assert.Equal(source.Token, token));
        Assert.Equal(0, service.SuccessfulLoads);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task LoadAsyncDoesNotCountFailedLoads()
    {
        var calls = 0;
        var service = new DashboardService(
            new ProfileClient(token =>
            {
                Interlocked.Increment(ref calls);
                return Task.FromResult(new Profile("Ada"));
            }),
            new OrdersClient(token =>
            {
                Interlocked.Increment(ref calls);
                return Task.FromException<IReadOnlyList<Order>>(
                    new InvalidOperationException("Orders service failed."));
            }),
            new RecommendationsClient(token =>
            {
                Interlocked.Increment(ref calls);
                return Task.FromResult<IReadOnlyList<Recommendation>>(
                    [new Recommendation("Concurrency in Practice")]);
            }));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.LoadAsync(CancellationToken.None));

        Assert.Equal(3, calls);
        Assert.Equal(0, service.SuccessfulLoads);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task SuccessfulLoadCounterIsThreadSafe()
    {
        const int requestCount = 2_000;
        var gate = new DependencyGate();
        var service = CreateGatedService(gate);

        var loads = Enumerable.Range(0, requestCount)
            .Select(_ => service.LoadAsync(CancellationToken.None))
            .ToArray();

        gate.Release();
        await Task.WhenAll(loads);

        Assert.Equal(requestCount, service.SuccessfulLoads);
    }

    private static DashboardService CreateGatedService(DependencyGate gate)
        => new(
            new ProfileClient(async token =>
            {
                await gate.WaitAsync(token);
                return new Profile("Ada");
            }),
            new OrdersClient(async token =>
            {
                await gate.WaitAsync(token);
                return [new Order(1)];
            }),
            new RecommendationsClient(async token =>
            {
                await gate.WaitAsync(token);
                return [new Recommendation("Concurrency in Practice")];
            }));

    private sealed class DependencyGate
    {
        private readonly TaskCompletionSource release = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly ConcurrentQueue<CancellationToken> receivedTokens = new();
        private int startedCount;

        public int StartedCount => Volatile.Read(ref startedCount);

        public IReadOnlyCollection<CancellationToken> ReceivedTokens => receivedTokens.ToArray();

        public async Task WaitAsync(CancellationToken cancellationToken)
        {
            receivedTokens.Enqueue(cancellationToken);
            Interlocked.Increment(ref startedCount);
            await release.Task.WaitAsync(cancellationToken);
        }

        public void Release() => release.TrySetResult();
    }

    private sealed class ProfileClient(
        Func<CancellationToken, Task<Profile>> load) : IProfileClient
    {
        public Task<Profile> GetProfileAsync(CancellationToken cancellationToken)
            => load(cancellationToken);
    }

    private sealed class OrdersClient(
        Func<CancellationToken, Task<IReadOnlyList<Order>>> load) : IOrdersClient
    {
        public Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken cancellationToken)
            => load(cancellationToken);
    }

    private sealed class RecommendationsClient(
        Func<CancellationToken, Task<IReadOnlyList<Recommendation>>> load) : IRecommendationsClient
    {
        public Task<IReadOnlyList<Recommendation>> GetRecommendationsAsync(
            CancellationToken cancellationToken)
            => load(cancellationToken);
    }
}
