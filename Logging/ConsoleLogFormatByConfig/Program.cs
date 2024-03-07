namespace ConsoleLogFormatByConfig;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        //The code will override the config in appsettings.json
        //builder.Logging.AddSimpleConsole(options =>
        //{
        //    options.IncludeScopes = true;
        //    options.SingleLine = true;
        //    options.TimestampFormat = "HH:mm:ss.fff ";
        // });

        var host = builder.Build();
        host.Run();
    }
}