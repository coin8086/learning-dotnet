using OpenTelemetry.Exporter;
using System.ComponentModel.DataAnnotations;

namespace UsingOpenTelemetry;

public class OtlpBaseOptions
{
    [Url]
    public string? EndPoint { get; set; }

    public OtlpExportProtocol? Protocol { get; set; }
}
