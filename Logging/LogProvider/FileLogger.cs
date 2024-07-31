using Microsoft.Extensions.Logging;
using System;


namespace LogProvider
{
    interface IFileLogWriter
    {
        void WriteMessage(string msg);
    }

    class FileLogger : ILogger
    {
        IFileLogWriter _logWriter;

        string _name;

        LogLevel _logLevel;

        public FileLogger(IFileLogWriter logWriter, string name, LogLevel logLevel)
        {
            _logWriter = logWriter;
            _name = name;
            _logLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotSupportedException();
        }

        //See NOTE on the same method of ConsoleLogger
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel && logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var msg = $"[{DateTimeOffset.UtcNow.ToString("o")}][{_name}][{logLevel}][{eventId}]: {formatter(state, exception)}\n";
                if (exception != null)
                {
                    msg += $"{exception}\n";
                }
                _logWriter.WriteMessage(msg);
            }
        }
    }
}
