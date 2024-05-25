//See https://devblogs.microsoft.com/dotnet/configureawait-faq/
//and https://learn.microsoft.com/en-us/archive/msdn-magazine/2011/february/msdn-magazine-parallel-computing-it-s-all-about-the-synchronizationcontext

namespace SynchronizationContextSample;

class MySynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        Console.WriteLine($"Post d={d}, state={state}");
        Console.WriteLine(Environment.StackTrace);
        base.Post(d, state);
        //d(state);
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        Console.WriteLine($"Send d={d}, state={state}");
        Console.WriteLine(Environment.StackTrace);
        base.Send(d, state);
        //d(state);
    }

    public override void OperationStarted()
    {
        Console.WriteLine("OperationStarted");
        Console.WriteLine(Environment.StackTrace);
        base.OperationStarted();
    }

    public override void OperationCompleted()
    {
        Console.WriteLine("OperationCompleted");
        Console.WriteLine(Environment.StackTrace);
        base.OperationCompleted();
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(SynchronizationContext.Current);
        //NOTE: Comment out SetSynchronizationContext and see the different output
        SynchronizationContext.SetSynchronizationContext(new MySynchronizationContext());
        Console.WriteLine(SynchronizationContext.Current);

        Console.WriteLine($"Main starts at thread {Thread.CurrentThread.ManagedThreadId}.");

        var task = Task.Run(() =>
        {
            Console.WriteLine($"I'm running at thread {Thread.CurrentThread.ManagedThreadId}.");
        });

        //NOTE: Compare the outputs of await vs synchronous Wait().
        //task.Wait();
        await task;

        Console.WriteLine($"Main ends at thread {Thread.CurrentThread.ManagedThreadId}.");
    }
}
