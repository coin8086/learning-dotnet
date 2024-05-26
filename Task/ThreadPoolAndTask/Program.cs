using System.Diagnostics;

namespace ThreadPoolSample;

class Program
{
    static void ShowThreadPoolStatus(bool noMinMax = false)
    {
        int workerThreads;
        int portThreads;

        if (!noMinMax)
        {
            ThreadPool.GetMinThreads(out workerThreads, out portThreads);
            Console.WriteLine($"Minimum threads: {workerThreads}, {portThreads}");

            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            Console.WriteLine($"Maximum threads: {workerThreads}, {portThreads}");
        }

        ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
        Console.WriteLine($"Available threads: {workerThreads}, {portThreads}");

        Console.WriteLine($"ThreadCount: {ThreadPool.ThreadCount}");
        Console.WriteLine($"PendingWorkItemCount: {ThreadPool.PendingWorkItemCount}");
        Console.WriteLine($"CompletedWorkItemCount: {ThreadPool.CompletedWorkItemCount}\n");
    }

    static void Main()
    {
        Console.WriteLine($"ProcessorCount: {Environment.ProcessorCount}");
        ShowThreadPoolStatus();

        var interval = 1000;
        using var timer = new Timer(_ => ShowThreadPoolStatus(true), null, interval, interval);

        var client = new HttpClient();
        var url = "https://www.microsoft.com/";
        var count = 1000;
        var tasks = new Task[count];
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < count; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                //NOTE: Compare different outputs by the following two lines separately
                //Thread.SpinWait(1000000);
                //using var response = await client.GetAsync(url);
                try
                {
                    using var response = client.GetAsync(url).Result;
                }
                catch (Exception) { }
            });
        }
        Task.WaitAll(tasks);
        stopWatch.Stop();

        timer.Change(int.MaxValue, int.MaxValue);
        ShowThreadPoolStatus();

        Console.WriteLine($"Total time in second: {stopWatch.Elapsed.TotalSeconds:f3}");
    }
}
