using Microsoft.Extensions.Options;

namespace ServiceConfiguration;


class WorkerOptions
{
    public string? Message { get; set; }
}

class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly WorkerOptions _options;

    public Worker(ILogger<Worker> logger, IOptions<WorkerOptions> opts)
    {
        _logger = logger;
        _options = opts.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("[{time}] {message}", DateTimeOffset.Now, _options.Message);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
