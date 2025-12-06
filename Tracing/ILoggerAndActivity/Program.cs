using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
    .AddSource("SampleActivitySource")
    .AddConsoleExporter()
    .Build();

var builder = Host.CreateApplicationBuilder(args);

// Configure console logging to include Activity IDs
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    //Scope must be enabled to show tracking info like TraceId, etc.
    options.IncludeScopes = true;
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

// Enable activity tracking in ILogger
builder.Logging.Configure(options =>
{
    options.ActivityTrackingOptions =
        ActivityTrackingOptions.TraceId |
        ActivityTrackingOptions.SpanId |
        ActivityTrackingOptions.ParentId;
});

using var host = builder.Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Create an ActivitySource (typical in libraries/apps)
var activitySource = new ActivitySource("SampleActivitySource");

// Start a root activity (this sets Activity.Current)
using (var activity = activitySource.StartActivity("RootOperation", ActivityKind.Internal))
{
    logger.LogInformation("Starting root operation");

    // Start a child activity
    using (var child = activitySource.StartActivity("ChildOperation", ActivityKind.Internal))
    {
        logger.LogInformation("Inside child operation");
    }

    logger.LogInformation("Finishing root operation");
}

logger.LogInformation("Done");