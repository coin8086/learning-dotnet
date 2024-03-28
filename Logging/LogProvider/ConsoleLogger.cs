using Microsoft.Extensions.Logging;
using System;


namespace LogProvider
{
    class ConsoleLogger : ILogger
    {
        string _name;

        LogLevel _logLevel;

        public ConsoleLogger(string name, LogLevel logLevel)
        {
            _name = name;
            _logLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel && logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var msg = $"[{DateTimeOffset.UtcNow.ToString("o")}][{_name}][{logLevel}][{eventId}]: {formatter(state, exception)}";
                if (exception != null)
                {
                    msg += $"\n{exception}";
                }
                Console.Error.WriteLine(msg);
            }
        }
    }
}
