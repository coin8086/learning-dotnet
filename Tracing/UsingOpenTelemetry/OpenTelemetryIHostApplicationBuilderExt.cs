using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace UsingOpenTelemetry;

public static class OpenTelemetryIHostApplicationBuilderExt
{
    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder, TelemteryOptions telemetryOptions,
        Action<MeterProviderBuilder>? configureMeter = null, Action<TracerProviderBuilder>? configureTracer = null, string? version = null)
    {
        var otel = builder.Services.AddOpenTelemetry();

        otel.ConfigureResource(resource =>
        {
            resource.AddService(serviceName: builder.Environment.ApplicationName, serviceVersion: version);
        });

        if (configureMeter != null)
        {
            otel.WithMetrics(metrics =>
            {
                if (telemetryOptions.ExportToConsole)
                {
                    metrics.AddConsoleExporter();
                }
                if (telemetryOptions.ExportToPrometheus)
                {
                    metrics.AddPrometheusExporter();
                }
                if (telemetryOptions.AzureMonitor != null)
                {
                    metrics.AddAzureMonitorMetricExporter(opts => opts.ConnectionString = telemetryOptions.AzureMonitor.ConnectionString);
                }
                configureMeter(metrics);
            });
        }

        if (configureTracer != null)
        {
            otel.WithTracing(tracing =>
            {
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
                configureTracer(tracing);
            });
        }

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
