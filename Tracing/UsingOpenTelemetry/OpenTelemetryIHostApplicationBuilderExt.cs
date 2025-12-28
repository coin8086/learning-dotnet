using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace UsingOpenTelemetry;

public static class OpenTelemetryIHostApplicationBuilderExt
{
    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder, TelemteryOptions options,
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
                if (options.ExportToConsole)
                {
                    metrics.AddConsoleExporter();
                }
                if (options.Otlp != null)
                {
                    var endPoint = options.Otlp.Metrics?.EndPoint ?? options.Otlp.EndPoint;
                    if (endPoint != null)
                    {
                        metrics.AddOtlpExporter(opts =>
                        {
                            opts.Endpoint = new Uri(endPoint);
                            var protocol = options.Otlp.Metrics?.Protocol ?? options.Otlp.Protocol;
                            if (protocol != null)
                            {
                                opts.Protocol = protocol.Value;
                            }
                        });
                    }
                }
                if (options.ExportToPrometheus)
                {
                    metrics.AddPrometheusExporter();
                }
                if (options.AzureMonitor != null)
                {
                    metrics.AddAzureMonitorMetricExporter(opts => opts.ConnectionString = options.AzureMonitor.ConnectionString);
                }
                configureMeter(metrics);
            });
        }

        if (configureTracer != null)
        {
            otel.WithTracing(tracing =>
            {
                if (options.ExportToConsole)
                {
                    tracing.AddConsoleExporter();
                }
                if (options.Otlp != null)
                {
                    var endPoint = options.Otlp.Trace?.EndPoint ?? options.Otlp.EndPoint;
                    if (endPoint != null)
                    {
                        tracing.AddOtlpExporter(opts =>
                        {
                            opts.Endpoint = new Uri(endPoint);
                            var protocol = options.Otlp.Trace?.Protocol ?? options.Otlp.Protocol;
                            if (protocol != null) {
                                opts.Protocol = protocol.Value;
                            }
                        });
                    }
                }
                if (options.AzureMonitor != null)
                {
                    tracing.AddAzureMonitorTraceExporter(opts => opts.ConnectionString = options.AzureMonitor.ConnectionString);
                }
                configureTracer(tracing);
            });
        }

        if (options?.AzureMonitor?.ExportLog ?? false)
        {
            builder.Logging.AddOpenTelemetry(opts =>
            {
                opts.AddAzureMonitorLogExporter(opts2 =>
                {
                    opts2.ConnectionString = options.AzureMonitor.ConnectionString;
                    opts2.EnableTraceBasedLogsSampler = false;
                });
            });
        }

        return builder;
    }
}
