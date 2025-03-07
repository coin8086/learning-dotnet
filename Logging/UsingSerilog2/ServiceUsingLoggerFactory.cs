namespace UsingSerilog2;

public interface IServiceUsingLoggerFactory
{
    void CallLogging();
}

public class ServiceUsingLoggerFactory : IServiceUsingLoggerFactory
{
    ILoggerFactory _loggerFactory;
    ILogger _logger;

    public ServiceUsingLoggerFactory(ILoggerFactory loggerFactory, ILogger<ServiceUsingLoggerFactory> logger)
    {
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    public void CallLogging()
    {
        _logger.LogInformation("Hello");
        var logger = _loggerFactory.CreateLogger("NewLogger");
        logger.LogInformation("Hello, too!");
    }
}
