using System.Diagnostics;

namespace TaskCancellation;

class Program
{
    static Task<int> CancellableBeforeRun(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            return 1;
        }, cancellationToken);
    }

    static Task<int> CancellableWhenRunning(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("No throw. You should not see this.");
            return 1;
        });
    }

    static async Task<int> CancellableWhenRunning2(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Console.WriteLine("No throw. You should not see this.");
        await Task.Delay(0);
        return 1;
    }

#pragma warning disable CS0162
    static Task<int> OtherExceptionWhenRunning()
    {
        return Task.Run(() =>
        {
            throw new ApplicationException("Some error when running.");
            return 1;
        });
    }

    static async Task<int> OtherExceptionWhenRunning2()
    {
        throw new ApplicationException("Some error when running.");
        await Task.Delay(0);
        return 1;
    }
#pragma warning restore CS0162

    static void CheckTaskStatus(Task task, string title)
    {
        Console.WriteLine($"\n===== {title} =====");
        Console.WriteLine($"Before Wait(): Status={task.Status}");
        try
        {
            task.Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
        }
        finally
        {
            Console.WriteLine($"After Wait(): Status={task.Status}");
        }
    }

    static void Main(string[] args)
    {
        var cts = new CancellationTokenSource(0);
        Debug.Assert(cts.Token.IsCancellationRequested);

        var task1 = CancellableBeforeRun(cts.Token);
        CheckTaskStatus(task1, "Cancel before task run");

        var task2 = CancellableWhenRunning(cts.Token);
        CheckTaskStatus(task2, "Cancel when task running");

        var task22 = CancellableWhenRunning2(cts.Token);
        CheckTaskStatus(task22, "Cancel when generated task running");

        var task3 = OtherExceptionWhenRunning();
        CheckTaskStatus(task3, "Other exception when task running");

        var task32 = OtherExceptionWhenRunning2();
        CheckTaskStatus(task32, "Other exception when generated task running");
    }
}
