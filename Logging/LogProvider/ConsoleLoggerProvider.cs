using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;


namespace LogProvider
{
    class ConsoleLoggerProvider : ILoggerProvider
    {
        private LogLevel _logLevel;

        private ConcurrentDictionary<string, ILogger> _loggers = new(StringComparer.OrdinalIgnoreCase);

        public ConsoleLoggerProvider(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (name) => new ConsoleLogger(name, _logLevel));
        }

        public void Dispose()
        {
        }
    }

    static class ConsoleLoggerProviderExtension
    {
        public static ILoggingBuilder AddConsoleLogger(this ILoggingBuilder builder, LogLevel logLevel = LogLevel.Trace)
        {
            var service = ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>(_ => new ConsoleLoggerProvider(logLevel));
            builder.Services.Add(service);
            return builder;
        }
    }
}
