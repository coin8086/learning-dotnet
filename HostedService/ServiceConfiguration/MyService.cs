namespace ServiceConfiguration;

class MyService
{
    public static string ConfigKey = "ServiceName";

    private ILogger _logger;

    public string Name { get; set; }

    public MyService(ILogger<MyService> logger, string name)
    {
        _logger = logger;
        Name = name;

        logger.LogInformation("I'm {name}", Name);
    }
}

static class ServiceCollectionMyServiceExtensions
{
    public static IServiceCollection AddMyService(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSingleton<MyService>(provider =>
        {
            var logger = provider.GetService<ILogger<MyService>>();
            var value = configuration[MyService.ConfigKey];
            return new MyService(logger, value);
        });
    }
}
