using System.Diagnostics;

namespace ThreadsAndNeedles.M02_Tasks;

public class TaskExperimentsTests
{
    [Fact(Skip = "Not Implemented")]
    public async Task CompleteLaterReturnsValueAfterAsynchronousDelay()
    {
        var stopwatch = Stopwatch.StartNew();

        var task = TaskExperiments.CompleteLater("done", TimeSpan.FromMilliseconds(80));
        var completedBeforeAwait = task.IsCompleted;
        var result = await task;

        stopwatch.Stop();

        Assert.False(completedBeforeAwait);
        Assert.Equal("done", result);
        Assert.True(stopwatch.ElapsedMilliseconds >= 50);
    }
}
