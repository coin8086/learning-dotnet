
namespace DynamicConfiguration;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly MyOptions _options;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.GetMyOptions("WorkerConfig");
        _logger.LogInformation("Options: {options}", _options);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
