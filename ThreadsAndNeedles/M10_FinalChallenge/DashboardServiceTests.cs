using System.Diagnostics;
using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class DashboardServiceTests
{
    [Fact]
    public async Task LoadAsyncLoadsAllDependenciesConcurrently()
    {
        var service = CreateService(TimeSpan.FromMilliseconds(140));
        var stopwatch = Stopwatch.StartNew();

        var dashboard = await service.LoadAsync(CancellationToken.None);

        stopwatch.Stop();

        Assert.Equal("Ada", dashboard.Profile.Name);
        Assert.Single(dashboard.Orders);
        Assert.Single(dashboard.Recommendations);
        Assert.Equal(1, service.SuccessfulLoads);
        Assert.True(
            stopwatch.Elapsed < TimeSpan.FromMilliseconds(260),
            $"Expected concurrent I/O, elapsed: {stopwatch.ElapsedMilliseconds} ms");
    }

    [Fact]
    public async Task LoadAsyncForwardsCancellation()
    {
        var service = CreateService(TimeSpan.FromSeconds(5));
        using var source = new CancellationTokenSource(40);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            service.LoadAsync(source.Token));

        Assert.Equal(0, service.SuccessfulLoads);
    }

    [Fact]
    public async Task LoadAsyncDoesNotCountFailedLoads()
    {
        var service = new DashboardService(
            new ProfileClient(TimeSpan.Zero),
            new FailingOrdersClient(),
            new RecommendationsClient(TimeSpan.Zero));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.LoadAsync(CancellationToken.None));

        Assert.Equal(0, service.SuccessfulLoads);
    }

    [Fact]
    public async Task SuccessfulLoadCounterIsThreadSafe()
    {
        var service = CreateService(TimeSpan.FromMilliseconds(1));

        await Task.WhenAll(
            Enumerable.Range(0, 500)
                .Select(_ => service.LoadAsync(CancellationToken.None)));

        Assert.Equal(500, service.SuccessfulLoads);
    }

    private static DashboardService CreateService(TimeSpan delay)
        => new(
            new ProfileClient(delay),
            new OrdersClient(delay),
            new RecommendationsClient(delay));

    private sealed class ProfileClient(TimeSpan delay) : IProfileClient
    {
        public async Task<Profile> GetProfileAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(delay, cancellationToken);
            return new Profile("Ada");
        }
    }

    private sealed class OrdersClient(TimeSpan delay) : IOrdersClient
    {
        public async Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(delay, cancellationToken);
            return [new Order(1)];
        }
    }

    private sealed class RecommendationsClient(TimeSpan delay) : IRecommendationsClient
    {
        public async Task<IReadOnlyList<Recommendation>> GetRecommendationsAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(delay, cancellationToken);
            return [new Recommendation("Concurrency in Practice")];
        }
    }

    private sealed class FailingOrdersClient : IOrdersClient
    {
        public Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken cancellationToken)
            => throw new InvalidOperationException("Orders service failed.");
    }
}
