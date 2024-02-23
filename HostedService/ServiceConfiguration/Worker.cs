using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ServiceConfiguration;

class WorkerOptions
{
    [Required]
    public string? Message { get; set; }

    [Required]
    public int Id { get; set; }
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
                _logger.LogInformation("[{time}] [{message}] [{id}] [{service}]", DateTimeOffset.Now, _options.Message, _options.Id, _service.Name);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}

static class ServiceCollectionWorkerServiceExtensions
{
    //NOTE: See more on option pattern and option validation at
    //https://learn.microsoft.com/en-us/dotnet/core/extensions/options
    public static IServiceCollection AddWorkerService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<Worker>();

        //NOTE: Configure<T> returns IServiceCollection and doesn't support option validation.
        //services.Configure<WorkerOptions>(configuration.GetSection(nameof(WorkerOptions)));

        //NOTE: To do option validation, use AddOptions***<T>, which returns OptionsBuilder<T>.
        //OptionsBuilder<T> has various methods to validate options.
        services.AddOptionsWithValidateOnStart<WorkerOptions>()
            .Bind(configuration.GetSection("Worker"))
            .ValidateDataAnnotations()  //Add package Microsoft.Extensions.Options.DataAnnotations to use this method.
            .Validate(opts => opts.Id > 100, "Id must be greater than 100!")
            .Validate(opts => opts.Message?.Length > 5, "Message length must be greater than 5!")
            .ValidateOnStart();         //Validation will be in runtime when without ValidateOnStart.

        return services;
    }
}
