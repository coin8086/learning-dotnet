using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace UsingOpenTelemetry;

public static class OpenTelemetryIHostApplicationBuilderExt
{
    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder, TelemteryOptions telemetryOptions)
    {
        var otel = builder.Services.AddOpenTelemetry();

        otel.ConfigureResource(resource => resource.AddService(serviceName: builder.Environment.ApplicationName));

        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddMeter(Globals.Meter.Name);

            if (telemetryOptions.ExportToConsole)
            {
                metrics.AddConsoleExporter();
            }
            if (telemetryOptions.ExportToPrometheus)
            {
                metrics.AddPrometheusExporter();
            }
            if (telemetryOptions.Otlp != null)
            {
                metrics.AddOtlpExporter(opts => opts.Endpoint = new Uri(telemetryOptions.Otlp.EndPoint));
            }
            if (telemetryOptions.AzureMonitor != null)
            {
                metrics.AddAzureMonitorMetricExporter(opts => opts.ConnectionString = telemetryOptions.AzureMonitor.ConnectionString);
            }
        });

        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource(Globals.Source.Name);

            if (telemetryOptions.ExportToConsole)
            {
                tracing.AddConsoleExporter();
            }
            if (telemetryOptions.Otlp != null)
            {
                tracing.AddOtlpExporter(opts => opts.Endpoint = new Uri(telemetryOptions.Otlp.EndPoint));
            }
            if (telemetryOptions.AzureMonitor != null)
            {
                tracing.AddAzureMonitorTraceExporter(opts => opts.ConnectionString = telemetryOptions.AzureMonitor.ConnectionString);
            }
        });

        if (telemetryOptions?.AzureMonitor?.ExportLog ?? false)
        {
            builder.Logging.AddOpenTelemetry(options =>
            {
                options.AddAzureMonitorLogExporter(opts =>
                {
                    opts.ConnectionString = telemetryOptions.AzureMonitor.ConnectionString;
                    opts.EnableTraceBasedLogsSampler = false;
                });
            });
        }

        return builder;
    }
}
