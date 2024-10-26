//See https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator

using Microsoft.Extensions.Logging;

namespace LoggingSrcGen;

class Program
{
    static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole();
        });

        ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
        logger.Log(LogLevel.Information, 100, "Bob", "Hello");
        logger.LogWarning(100, "Bob", "Hello");
    }
}

public static partial class CustomLog
{
    [LoggerMessage("[{id}, {name}]: {msg}")]
    public static partial void Log(this ILogger logger, LogLevel level, int id, string name, string msg);


    [LoggerMessage(LogLevel.Warning, "[{id}, {name}]: {msg}")]
    public static partial void LogWarning(this ILogger logger, int id, string name, string msg);
}
