//See https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-prgrja-example
//and https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-enable?tabs=net
//and https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-configuration?tabs=net
//and https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-add-modify?tabs=net

namespace UsingOpenTelemetry;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddHttpClient();

        var telemetryOptions = builder.Configuration.GetTelemteryOptions("Telemetry");
        if (telemetryOptions != null)
        {
            builder.AddOpenTelemetry(telemetryOptions);
        }

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
