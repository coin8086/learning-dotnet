namespace ServiceConfiguration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        //NOTE: Options can be get without using DI.
        var opts = builder.Configuration.GetSection("Worker").Get<WorkerOptions>();
        Console.WriteLine($"Id={opts.Id}, Message={opts.Message}");

        builder.Services.AddMyService(builder.Configuration);

        builder.Services.AddWorkerService(builder.Configuration);

        var switchMappings = new Dictionary<string, string>()
        {
            { "-m", "WorkerOptions:Message" },
            { "-n", MyService.ConfigKey }
        };
        builder.Configuration.AddCommandLine(args, switchMappings);

        var host = builder.Build();
        host.Run();
    }
}
