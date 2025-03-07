using Microsoft.Extensions.Logging;

namespace CustomLoggerProvider;

public class MyLogger : ILogger
{
    private readonly string _category;

    public MyLogger(string category)
    {
        _category = category;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotSupportedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        //TODO: What about the formatter? How to customize it?
        var msg = $"[{DateTimeOffset.UtcNow.ToString("HH:mm:ss.fff")}][{_category}][{logLevel}]: {formatter(state, exception)}";
        Console.Error.WriteLine(msg);
    }
}
