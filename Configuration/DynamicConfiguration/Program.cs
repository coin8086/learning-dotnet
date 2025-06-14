using System.Text.Json;

namespace DynamicConfiguration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        var config = builder.Configuration;
        var myOptions = config.GetMyOptions("WorkerConfig");
        Console.WriteLine(myOptions);

        var host = builder.Build();
        host.Run();
    }
}
