namespace ServiceConfiguration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        //NOTE: Options can be get without using DI.
        var opts = builder.Configuration.GetSection("Worker").Get<WorkerOptions>();
        Console.WriteLine($"Id={opts?.Id}, Message={opts?.Message}");

        Console.WriteLine("Config Sources:");
        foreach (var source in builder.Configuration.Sources)
        {
            Console.WriteLine(source.ToString());
        }

        builder.Services.AddMyService(builder.Configuration);

        builder.Services.AddWorkerService(builder.Configuration);

        var host = builder.Build();
        host.Run();
    }
}
