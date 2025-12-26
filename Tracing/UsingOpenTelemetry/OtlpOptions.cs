using System.ComponentModel.DataAnnotations;

namespace UsingOpenTelemetry;

public class OtlpOptions
{
    [Required]
    [Url]
    public string EndPoint { get; set; } = default!;
}
