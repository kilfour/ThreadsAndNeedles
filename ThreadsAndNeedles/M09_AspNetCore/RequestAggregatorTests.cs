namespace ThreadsAndNeedles.M09_AspNetCore;

public class RequestAggregatorTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task LoadAsyncStartsIndependentCallsConcurrently()
    {
        var aggregator = new RequestAggregator();
        var release = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var firstStarted = false;
        var secondStarted = false;

        async Task<string> LoadFirst(CancellationToken token)
        {
            firstStarted = true;
            await release.Task.WaitAsync(token);
            return "A";
        }

        async Task<string> LoadSecond(CancellationToken token)
        {
            secondStarted = true;
            await release.Task.WaitAsync(token);
            return "B";
        }

        var loadTask = aggregator.LoadAsync(
            LoadFirst,
            LoadSecond,
            CancellationToken.None);
        var bothStartedBeforeRelease = firstStarted && secondStarted;

        release.SetResult();
        var result = await loadTask;

        Assert.Equal(new AggregatedResult("A", "B"), result);
        Assert.True(bothStartedBeforeRelease, "Both calls must start before either one completes.");
    }

    [Fact(Skip = "Not Implemented")]
    public async Task LoadAsyncForwardsCancellation()
    {
        var aggregator = new RequestAggregator();
        using var source = new CancellationTokenSource();
        await source.CancelAsync();
        CancellationToken? firstToken = null;
        CancellationToken? secondToken = null;

        Task<string> LoadFirst(CancellationToken token)
        {
            firstToken = token;
            return token.IsCancellationRequested
                ? Task.FromCanceled<string>(token)
                : Task.FromResult("not cancelled");
        }

        Task<string> LoadSecond(CancellationToken token)
        {
            secondToken = token;
            return token.IsCancellationRequested
                ? Task.FromCanceled<string>(token)
                : Task.FromResult("not cancelled");
        }

        var loadTask = aggregator.LoadAsync(LoadFirst, LoadSecond, source.Token);

        Assert.Equal(source.Token, firstToken);
        Assert.Equal(source.Token, secondToken);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => loadTask);
    }
}
