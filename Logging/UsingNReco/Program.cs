using NReco.Logging.File;

namespace UsingNReco;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddLogging(loggingBuilder => {
            var loggingSection = builder.Configuration.GetSection("Logging");
            loggingBuilder.AddFile(loggingSection);
        });

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

    public static void ThrowException(string msg)
    {
        throw new ApplicationException(msg);
    }
}
