using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTimer;

class Program
{
    static void Main(string[] args)
    {
        using var timer = new Timer(TimerCallback, null, 1000, 1000);
        using var timer2 = new Timer(TimerCallbackAsync, null, 1000, 1000);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(false);
    }

    static void TimerCallback(object? _)
    {
        Console.WriteLine($"Timer 1: {DateTimeOffset.Now}");
    }

    static async void TimerCallbackAsync(object? _)
    {
        await Task.Delay(1000);
        Console.WriteLine($"Timer 2: {DateTimeOffset.Now}");
    }
}
