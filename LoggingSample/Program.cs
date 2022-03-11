using Microsoft.Extensions.Logging;
using System;

namespace LoggingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("LoggingSample.Program", LogLevel.Information)
                    .AddConsole()
                    //NOTE: the cusomt file logger doesn't pick up log settings from appsettings.json.
                    //And, its log level is only determined by the argument passed in here.
                    .AddFileLogger("LogFile.txt", LogLevel.Error);
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogDebug("Example debug message");
            logger.LogInformation("Example info message");
            logger.LogWarning("Example warning message");
            logger.LogError("Example error message");
            logger.LogError(100, new ArgumentNullException(), "Example exception {msg}", "This is bad!");
        }
    }
}
