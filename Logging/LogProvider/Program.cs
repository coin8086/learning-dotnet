using Microsoft.Extensions.Logging;
using System;

namespace LogProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .ClearProviders()
                    //NOTE: The log level here for AddFilter is like the value/level for System.Diagnostics.SourceSwitch,
                    //which affects all TraceSources having the Switch.
                    .AddFilter("LogProvider.Program", LogLevel.Information)
                    .AddConsoleLogger()
                    //NOTE: The log level here set on the logger (which determines the result of IsEnabled()) is like
                    //the TraceListener's Filter property, which is just another layer of screening beyond the Filter on category.
                    .AddFileLogger("LogFile.txt", LogLevel.Error);
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogDebug("Example debug message");
            logger.LogInformation("Example info message");
            logger.LogWarning("Example warning message");
            logger.LogError("Example error message");
            logger.LogError(100, new NotSupportedException("some error"), "Example exception {msg}", "This is bad!");
        }
    }
}
