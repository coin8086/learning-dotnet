using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CustomLoggerProvider;

public class MyLoggerProvider : ILoggerProvider
{
    private ConcurrentDictionary<string, ILogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, (name) => new MyLogger(name));
    }

    public void Dispose()
    {
    }
}

public static class MyLoggerProviderExtensions
{
    public static ILoggingBuilder AddMyLogger(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        return builder;
    }
}
