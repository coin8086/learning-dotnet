using Serilog;

namespace UsingSerilog2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services));

        builder.Services.AddControllers();

        builder.Services.AddSingleton<IServiceUsingLoggerFactory, ServiceUsingLoggerFactory>();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
