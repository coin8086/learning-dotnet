//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging
//and https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider

using Microsoft.Extensions.Logging;

namespace CustomLoggerProvider;

class Program
{
    static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .ClearProviders()
                .AddMyLogger()
                .AddFilter(typeof(Program).Name, LogLevel.Information);
        });

        //The logger returned from CreateLogger is in fact a wrapper of loggers from all logger providers,
        //including the custom provider.
        ILogger logger = loggerFactory.CreateLogger<Program>();
        Console.WriteLine($"Logger type: {logger.GetType().FullName}");
        Console.WriteLine($"Is debug level enabled? {logger.IsEnabled(LogLevel.Debug)}");
        Console.WriteLine($"Is information level enabled? {logger.IsEnabled(LogLevel.Information)}");

        logger.LogDebug("A debug message.");
        logger.LogInformation("An information message.");
        logger.LogWarning("A warning message.");
        logger.LogError("An error message.");

        try
        {
            RaiseException();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception!");
        }
    }

    static void RaiseException()
    {
        throw new ApplicationException("An error happened!");
    }
}
