using System.Diagnostics;
using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class AsyncPitfallsExercisesTests
{
    [Fact]
    public async Task LoadWithoutBlockingAsyncReturnsLoaderResult()
    {
        var result = await AsyncPitfallsExercises.LoadWithoutBlockingAsync(
            async () =>
            {
                await Task.Delay(10);
                return "ok";
            });

        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task SaveBeforeReturningAsyncWaitsForSave()
    {
        var saved = false;

        await AsyncPitfallsExercises.SaveBeforeReturningAsync(
            async () =>
            {
                await Task.Delay(30);
                saved = true;
            });

        Assert.True(saved);
    }

    [Fact]
    public async Task LoadIndependentValuesAsyncRunsConcurrently()
    {
        var stopwatch = Stopwatch.StartNew();

        async Task<string> Load(string value)
        {
            await Task.Delay(120);
            return value;
        }

        var result = await AsyncPitfallsExercises.LoadIndependentValuesAsync(
            () => Load("first"),
            () => Load("second"));

        stopwatch.Stop();

        Assert.Equal(("first", "second"), result);
        Assert.True(stopwatch.Elapsed < TimeSpan.FromMilliseconds(210));
    }
}
