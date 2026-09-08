namespace ThreadsAndNeedles.M03_AsyncAwait;

public class AsyncAwaitExercisesTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task LoadAndTransformAsyncLoadsOnceAndTransformsValue()
    {
        var calls = 0;

        Task<string> Loader()
        {
            calls++;
            return Task.FromResult("  hello async  ");
        }

        var result = await AsyncAwaitExercises.LoadAndTransformAsync(Loader);

        Assert.Equal("HELLO ASYNC", result);
        Assert.Equal(1, calls);
    }
}
