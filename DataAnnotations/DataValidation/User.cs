using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DataValidation;

class User
{
    [Required]
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public override string ToString()
    {
        var opts = new JsonSerializerOptions() { WriteIndented = true };
        var json = JsonSerializer.Serialize(this, opts);
        return json;
    }
}
