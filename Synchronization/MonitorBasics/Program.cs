namespace MonitorBasics;

using System;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    private static readonly object _lock = new object();

    public static async Task Main(string[] args)
    {
        var task1 = AccessResourceAsync("Task 1");
        var task2 = AccessResourceAsync("Task 2");

        await Task.WhenAll(task1, task2);
    }

    private static async Task AccessResourceAsync(string taskName)
    {
        await Task.Yield();
        Console.WriteLine($"{taskName} is waiting to enter the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");

        try
        {
            Monitor.Enter(_lock);

            Console.WriteLine($"{taskName} has entered the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");

            // Simulate some work with async delay
            Task.Delay(1000).Wait();

            /*
             * NOTE
             * 
             * The await call is an error since the thread before and after await may be different. That means Monitor.Exit 
             * may be called on a thread other than the thread that Monitor.Enter was called on, which means the the thread
             * that owns the lock may be different than the one that releases it.
             * 
             * await Task.Delay(1000);
             */

            Console.WriteLine($"{taskName} is leaving the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");
        }
        finally
        {
            Monitor.Exit(_lock);
            Console.WriteLine($"{taskName} has exited the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");
        }
    }

    /*
     * NOTE
     * 
     * An equivalent to AccessResourceAsync. "lock" statement is recommended instead of a pair of Enter and Exit, for the
     * compiler will raise error for await inside a code block of "lock".
     */
    private static async Task AccessResource2Async(string taskName)
    {
        await Task.Yield();
        Console.WriteLine($"{taskName} is waiting to enter the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");

        lock (_lock)
        {
            Console.WriteLine($"{taskName} has entered the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");

            // Simulate some work with async delay
            Task.Delay(1000).Wait();

            //The compiler will raise an error for the await statement.
            //await Task.Delay(1000);

            Console.WriteLine($"{taskName} is leaving the critical section on thread {Thread.CurrentThread.ManagedThreadId}.");
        }
    }
}
