using System.ComponentModel.DataAnnotations;

namespace UsingOpenTelemetry;

public class TelemteryOptions
{
    //Export metrics and trace to console
    public bool ExportToConsole { get; set; } = false;

    //Export metrics to Prometheus
    public bool ExportToPrometheus { get; set; } = false;

    //Export trace in OTLP
    public OtlpOptions? Otlp { get; set; }

    //Export metrics, trace and optionally logs to Azure Monitor
    public AzureMonitorOptions? AzureMonitor { get; set; }
}

public static class TelemteryOptionsConfigurationManagerExtensions
{
    public static TelemteryOptions? GetTelemteryOptions(this ConfigurationManager configuration, string configSectionPath)
    {
        var options = configuration.GetSection(configSectionPath).Get<TelemteryOptions>();
        if (options == null)
        {
            return null;
        }

        Validator.ValidateObject(options, new ValidationContext(options), true);
        return options;
    }
}