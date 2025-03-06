//See https://github.com/serilog/serilog-aspnetcore
//and https://github.com/serilog/serilog-settings-configuration
//and https://github.com/serilog/serilog/wiki/Formatting-Output
//and https://github.com/serilog/serilog/wiki/Configuration-Basics

using Serilog;

namespace UsingSerilog;

public class Program
{
    public static void Main(string[] args)
    {
        //The bootstrap logger can be configurated only by code since the configuration service is
        //not loaded at this point.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        
        try
        {
            Log.Information("App starts.");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSerilog((services, lc) => lc
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services));

            var app = builder.Build();

            app.MapGet("/", (ILogger<Program> logger) =>
            {
                logger.LogDebug("Handling /");
                logger.LogInformation("Hello!");
                return "Hello World!";
            });

            app.MapGet("/error", (ILogger<Program> logger) =>
            {
                logger.LogDebug("Handling /error");
                try
                {
                    ThrowException("Some error");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error!");
                }
                return "Error!";
            });

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "App had error on starting.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static void ThrowException(string msg)
    {
        throw new ApplicationException(msg);
    }
}
