using System;
using System.Threading.Tasks;
using System.Timers;

namespace SystemTimer;

class Program
{
    static void Main(string[] args)
    {
        using var timer = new Timer(1000);
        timer.Elapsed += TimerElapsed;
        timer.Elapsed += TimerElapsedAsync;
        timer.Start();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(false);
    }

    private static void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Console.WriteLine($"Timer 1: {DateTimeOffset.Now}");
    }

    private static async void TimerElapsedAsync(object? sender, ElapsedEventArgs e)
    {
        await Task.Delay(1000);
        Console.WriteLine($"Timer 2: {DateTimeOffset.Now}");
    }
}
