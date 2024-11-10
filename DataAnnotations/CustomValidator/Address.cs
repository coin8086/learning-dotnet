using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CustomValidator;

public class Address
{
    [Required]
    public string City { get; set; } = default!;

    [Required]
    public string State { get; set; } = default!;

    public string? Detail { get; set; }

    public override string ToString()
    {
        var opts = new JsonSerializerOptions() { WriteIndented = true };
        var json = JsonSerializer.Serialize(this, opts);
        return json;
    }
}
