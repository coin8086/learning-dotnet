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
        //NOTE: MyService depends on a single string option. So the general Option Pattern doesn't work.
        //Here a factory method is used to create an instance of MyService. Note how the configuration 
        //is read from IConfiguration directly.
        return services.AddSingleton<MyService>(provider =>
        {
            var logger = provider.GetService<ILogger<MyService>>();
            var value = configuration[MyService.ConfigKey];
            return new MyService(logger, value);
        });
    }
}
