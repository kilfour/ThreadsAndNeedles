namespace ConcurrencyLab;

public sealed record Profile(string Name);
public sealed record Order(int Id);
public sealed record Recommendation(string Title);

public sealed record Dashboard(
    Profile Profile,
    IReadOnlyList<Order> Orders,
    IReadOnlyList<Recommendation> Recommendations);

public interface IProfileClient
{
    Task<Profile> GetProfileAsync(CancellationToken cancellationToken);
}

public interface IOrdersClient
{
    Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken cancellationToken);
}

public interface IRecommendationsClient
{
    Task<IReadOnlyList<Recommendation>> GetRecommendationsAsync(CancellationToken cancellationToken);
}

public sealed class DashboardService(
    IProfileClient profileClient,
    IOrdersClient ordersClient,
    IRecommendationsClient recommendationsClient)
{
    private int successfulLoads;

    public int SuccessfulLoads => successfulLoads;

    public async Task<Dashboard> LoadAsync(CancellationToken cancellationToken)
    {
        // TODO: run independent I/O concurrently, forward cancellation and count only successful loads.
        var profile = await profileClient.GetProfileAsync(cancellationToken);
        var orders = await ordersClient.GetOrdersAsync(cancellationToken);
        var recommendations = await recommendationsClient.GetRecommendationsAsync(cancellationToken);

        successfulLoads++;

        return new Dashboard(profile, orders, recommendations);
    }
}
