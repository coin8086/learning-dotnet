//See https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-instrumentation-walkthroughs

using System.Diagnostics;

namespace MyLib;

public class MyWorker
{
    static ActivitySource _source = new ActivitySource(typeof(MyWorker).FullName!, "1.0.0");

    public async Task DoSomeWork(string foo, int bar)
    {
        using (var activity = _source.StartActivity("SomeWork"))
        {
            activity?.SetTag("foo", foo);
            activity?.SetTag("bar", bar);
            await StepOne().ConfigureAwait(false);
            activity?.AddEvent(new ActivityEvent("Step one done"));
            await StepTwo().ConfigureAwait(false);
            activity?.AddEvent(new ActivityEvent("Step two done"));
        }
    }

    async Task StepOne()
    {
        using (var activity = _source.StartActivity("StepOne"))
        {
            await Task.Delay(500).ConfigureAwait(false);
        }
    }

    async Task StepTwo()
    {
        using (var activity = _source.StartActivity("StepTwo"))
        {
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}
