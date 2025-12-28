using System.ComponentModel.DataAnnotations;

namespace UsingOpenTelemetry;

public class OtlpOptions : OtlpBaseOptions
{
    public OtlpBaseOptions? Metrics { get; set; }

    public OtlpBaseOptions? Trace { get; set; }
}
