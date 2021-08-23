using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WorkerSample
{
    public class WorkerOptions
    {
        public string Name { get; set; }

        //Try to set the Range to (1, 100) and see what will happen
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Age { get; set; }
    }

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IOptions<WorkerOptions> _options;
        private IOptionsMonitor<WorkerOptions> _optionsMonitor;

        public Worker(ILogger<Worker> logger, IOptions<WorkerOptions> options, IOptionsMonitor<WorkerOptions> optionsMonitor)
        {
            _logger = logger;
            _options = options;
            _optionsMonitor = optionsMonitor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Try to change the "Name" in "appsettings.{env}.json".
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    //Validation happens when getting _options.Value or _optionsMonitor.CurrentValue.
                    _logger.LogInformation($"Worker Name = {_options.Value.Name}, Current Name = {_optionsMonitor.CurrentValue.Name}");
                    _logger.LogInformation($"Worker Age = {_options.Value.Age}, Current Age = {_optionsMonitor.CurrentValue.Age}");
                }
                catch (OptionsValidationException ex)
                {
                    foreach (var failure in ex.Failures)
                    {
                        _logger.LogError(failure);
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
