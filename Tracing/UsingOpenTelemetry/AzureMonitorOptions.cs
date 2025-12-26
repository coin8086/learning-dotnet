using System.ComponentModel.DataAnnotations;

namespace UsingOpenTelemetry;

public class AzureMonitorOptions
{
    [Required]
    public string ConnectionString { get; set; } = default!;

    public bool ExportLog { get; set; } = false;
}
