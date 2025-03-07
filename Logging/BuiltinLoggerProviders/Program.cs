//See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging

namespace BuiltinLoggerProviders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        try
        {
            app.MapGet("/", (ILogger<Program> logger) =>
            {
                logger.LogInformation("Hello");
                return "Hello!";
            });

            app.Logger.LogInformation("App started.");
            app.Run();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "App error!");
        }
        finally
        {
            app.Logger.LogInformation("App ended.");
        }
    }
}
