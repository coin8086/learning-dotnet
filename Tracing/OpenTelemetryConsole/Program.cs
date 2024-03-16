//See https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-collection-walkthroughs#collect-traces-using-opentelemetry

using MyLib;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OpenTelemetryConsole;

class Program
{
    static ActivitySource _source = new ActivitySource(typeof(Program).FullName!, "1.0.0");

    static async Task Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
            .AddSource(typeof(Program).FullName!)
            .AddSource(typeof(MyWorker).FullName!)
            .AddConsoleExporter()
            .Build();

        using (var activity = _source.StartActivity("Main"))
        {
            Console.WriteLine("Begin some work");

            var worker = new MyWorker();
            await worker.DoSomeWork("banana", 8).ConfigureAwait(false);

            Console.WriteLine("Some work done");
        }
    }
}
