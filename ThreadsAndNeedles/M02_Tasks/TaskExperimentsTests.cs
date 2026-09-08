using System.Diagnostics;
using ConcurrencyLab;

namespace ConcurrencyLab.Tests;

public class TaskExperimentsTests
{
    [Fact]
    public async Task CompleteLaterReturnsValueAfterDelay()
    {
        var stopwatch = Stopwatch.StartNew();

        var result = await TaskExperiments.CompleteLater("done", TimeSpan.FromMilliseconds(80));

        stopwatch.Stop();

        Assert.Equal("done", result);
        Assert.True(stopwatch.ElapsedMilliseconds >= 50);
    }
}
