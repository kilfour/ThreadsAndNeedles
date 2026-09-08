namespace ConcurrencyLab;

public static class ThreadExperiments
{
    public static int CurrentThreadId()
        => Environment.CurrentManagedThreadId;

    public static int RunOnDedicatedThread(Action action)
    {
        // TODO: run action on a dedicated Thread and return that thread's managed id.
        action();
        return Environment.CurrentManagedThreadId;
    }
}
