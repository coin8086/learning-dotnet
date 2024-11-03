//See https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.yield?view=net-8.0

namespace TaskYield;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(DateTime.Now);
        await LongRunningTask();
        Console.WriteLine(DateTime.Now);
        await LongRunningTaskYield();
        Console.WriteLine(DateTime.Now);
    }

    static Task LongRunningTask()
    {
        Console.WriteLine($"{DateTime.Now}: LongRunningTask start: {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(2000);
        Console.WriteLine($"{DateTime.Now}: LongRunningTask end: {Thread.CurrentThread.ManagedThreadId}");
        return Task.CompletedTask;
    }

    static async Task LongRunningTaskYield()
    {
        Console.WriteLine($"{DateTime.Now}: LongRunningTaskYield start: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Yield();
        Thread.Sleep(2000);
        Console.WriteLine($"{DateTime.Now}: LongRunningTaskYield end: {Thread.CurrentThread.ManagedThreadId}");
    }
}
