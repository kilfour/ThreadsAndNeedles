namespace ThreadsAndNeedles.M04_ConcurrentAsync;

public class ConcurrentAsyncExercisesTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task LoadBothAsyncStartsBothOperationsBeforeWaiting()
    {
        var release = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var firstStarted = false;
        var secondStarted = false;

        async Task<string> LoadFirst()
        {
            firstStarted = true;
            await release.Task;
            return "A";
        }

        async Task<string> LoadSecond()
        {
            secondStarted = true;
            await release.Task;
            return "B";
        }

        var loadTask = ConcurrentAsyncExercises.LoadBothAsync(LoadFirst, LoadSecond);
        var bothStartedBeforeRelease = firstStarted && secondStarted;

        release.SetResult();
        var result = await loadTask;

        Assert.Equal(("A", "B"), result);
        Assert.True(bothStartedBeforeRelease, "Both loaders must start before either one completes.");
    }
}
