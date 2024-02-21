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
    private readonly MyService _service;

    public Worker(ILogger<Worker> logger, IOptions<WorkerOptions> opts, MyService service)
    {
        _logger = logger;
        _options = opts.Value;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("[{time}] [{message}] [{service}]", DateTimeOffset.Now, _options.Message, _service.Name);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
