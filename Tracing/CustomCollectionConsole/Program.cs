//See https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-collection-walkthroughs

using MyLib;
using System.Diagnostics;

namespace CustomCollectionConsole;

class Program
{
    static ActivitySource _source = new ActivitySource(typeof(Program).FullName!, "1.0.0");

    static async Task Main(string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;

        Console.WriteLine("         {0,-15} {1,-60} {2,-15}", "OperationName", "Id", "Duration");
        ActivitySource.AddActivityListener(new ActivityListener()
        {
            ShouldListenTo = (source) => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStarted = activity => Console.WriteLine("Started: {0,-15} {1,-60}", activity.OperationName, activity.Id),
            ActivityStopped = activity => Console.WriteLine("Stopped: {0,-15} {1,-60} {2,-15}", activity.OperationName, activity.Id, activity.Duration)
        });

        using (var activity = _source.StartActivity("Main"))
        {
            Console.WriteLine("Begin some work");

            var worker = new MyWorker();
            await worker.DoSomeWork("banana", 8).ConfigureAwait(false);

            Console.WriteLine("Some work done");
        }
    }
}
