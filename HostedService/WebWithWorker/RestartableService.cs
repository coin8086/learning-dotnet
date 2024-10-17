namespace WebWithWorker;

public abstract class RestartableService : BackgroundService
{
    public bool IsRunning => !(ExecuteTask?.IsCompleted) ?? false;

    private object _lock = new object();

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (IsRunning)
            {
                return Task.CompletedTask;
            }
            return base.StartAsync(cancellationToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (!IsRunning)
            {
                return Task.CompletedTask;
            }
            return base.StopAsync(cancellationToken);
        }
    }
}
