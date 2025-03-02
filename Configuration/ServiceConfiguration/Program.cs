namespace ServiceConfiguration;

using static ConfigurationCommon.Utils;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        ShowOptions<WorkerOptions>(builder.Configuration, "Worker");

        ShowConfigurationSources(builder.Configuration);

        ShowConfigurationProperties(builder.Configuration);

        builder.Services.AddWorkerService();

        var host = builder.Build();
        host.Run();
    }
}
