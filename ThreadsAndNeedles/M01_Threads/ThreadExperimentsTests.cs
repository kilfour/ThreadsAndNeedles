namespace ThreadsAndNeedles.M01_Threads;

public class ThreadExperimentsTests
{
    [Fact]
    public void CurrentThreadIdReturnsAValidThreadId()
    {
        Assert.True(ThreadExperiments.CurrentThreadId() > 0);
    }

    [Fact(Skip = "Not Implemented")]
    public void RunOnDedicatedThreadUsesAnotherThread()
    {
        var callerThreadId = Environment.CurrentManagedThreadId;
        var actionThreadId = 0;

        var returnedThreadId = ThreadExperiments.RunOnDedicatedThread(
            () => actionThreadId = Environment.CurrentManagedThreadId);

        Assert.NotEqual(callerThreadId, actionThreadId);
        Assert.Equal(actionThreadId, returnedThreadId);
    }
}
