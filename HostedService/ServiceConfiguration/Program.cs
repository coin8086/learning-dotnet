namespace ServiceConfiguration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<Worker>();

        builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(nameof(WorkerOptions)));

        var switchMappings = new Dictionary<string, string>()
        {
            { "-m", "WorkerOptions:Message" }
        };
        builder.Configuration.AddCommandLine(args, switchMappings);

        var host = builder.Build();
        host.Run();
    }
}
