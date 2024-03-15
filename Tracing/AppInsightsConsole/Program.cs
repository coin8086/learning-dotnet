//See https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#net-corenet-framework-console-application

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppInsightsConsole;

class Program
{
    static async Task Main(string[] args)
    {
        // Create the DI container.
        IServiceCollection services = new ServiceCollection();

        // Being a regular console app, there is no appsettings.json or configuration providers enabled by default.
        // Hence instrumentation key/ connection string and any changes to default logging level must be specified here.
        services.AddLogging(builder =>
        {
            builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("Default", LogLevel.Information);
            builder.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "HH:mm:ss.fff ";
            });
        });

        //Provide the connection string by environment var APPLICATIONINSIGHTS_CONNECTION_STRING or in code
        //services.AddApplicationInsightsTelemetryWorkerService(options =>
        //{
        //    options.ConnectionString = "";
        //});
        services.AddApplicationInsightsTelemetryWorkerService();

        // Build ServiceProvider.
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        // Obtain logger instance from DI.
        ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        // Obtain TelemetryClient instance from DI, for additional manual tracking or to flush.
        var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

        var httpClient = new HttpClient();
        var stop = new CancellationTokenSource();

        Console.WriteLine("Press any key to exit...");
        _ = Task.Run(() =>
        {
            Console.ReadKey(true);
            stop.Cancel();
        });

        while (!stop.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at {time}", DateTimeOffset.Now);

            try
            {
                // Replace with a name which makes sense for this operation.
                using (telemetryClient.StartOperation<RequestTelemetry>("operation"))
                {
                    logger.LogWarning("A sample warning message.");
                    logger.LogInformation("Calling bing.com");
                    var res = await httpClient.GetAsync("https://bing.com", stop.Token);
                    logger.LogInformation("Calling bing completed with status {code}", res.StatusCode);
                    telemetryClient.TrackEvent("Bing call event completed");
                }

                await Task.Delay(1000, stop.Token);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Worker is stopping at {time}", DateTimeOffset.Now);
                break;
            }
        }

        // Explicitly call Flush() followed by sleep is required in console apps.
        // This is to ensure that even if application terminates, telemetry is sent to the back-end.
        telemetryClient.Flush();
        Task.Delay(5000).Wait();
        logger.LogInformation("Worker stopped at {time}", DateTimeOffset.Now);
    }
}
