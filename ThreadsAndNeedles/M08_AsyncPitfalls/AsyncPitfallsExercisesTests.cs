namespace ThreadsAndNeedles.M08_AsyncPitfalls;

public class AsyncPitfallsExercisesTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task LoadWithoutBlockingAsyncReturnsBeforeLoaderCompletes()
    {
        var loaderResult = new TaskCompletionSource<string>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var invocation = Task.Factory.StartNew(
            () => AsyncPitfallsExercises.LoadWithoutBlockingAsync(() => loaderResult.Task),
            CancellationToken.None,
            TaskCreationOptions.DenyChildAttach,
            TaskScheduler.Default);

        Task<string>? returnedTask = null;

        try
        {
            returnedTask = await invocation.WaitAsync(TimeSpan.FromSeconds(2));
            Assert.False(returnedTask.IsCompleted);
        }
        finally
        {
            loaderResult.TrySetResult("ok");
        }

        var result = await returnedTask;

        Assert.Equal("ok", result);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task SaveBeforeReturningAsyncWaitsForSave()
    {
        var saveFinished = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var saveTask = AsyncPitfallsExercises.SaveBeforeReturningAsync(
            () => saveFinished.Task);
        var returnedBeforeSaveFinished = saveTask.IsCompleted;

        saveFinished.SetResult();
        await saveTask;

        Assert.False(returnedBeforeSaveFinished);
    }

    [Fact(Skip = "Not Implemented")]
    public async Task LoadIndependentValuesAsyncRunsConcurrently()
    {
        var release = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var firstStarted = false;
        var secondStarted = false;

        async Task<string> LoadFirst()
        {
            firstStarted = true;
            await release.Task;
            return "first";
        }

        async Task<string> LoadSecond()
        {
            secondStarted = true;
            await release.Task;
            return "second";
        }

        var loadTask = AsyncPitfallsExercises.LoadIndependentValuesAsync(LoadFirst, LoadSecond);
        var bothStartedBeforeRelease = firstStarted && secondStarted;

        release.SetResult();
        var result = await loadTask;

        Assert.Equal(("first", "second"), result);
        Assert.True(bothStartedBeforeRelease, "Both loaders must start before either one completes.");
    }
}
