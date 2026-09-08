using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class AsyncAwaitExercisesTests
{
    [Fact]
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
