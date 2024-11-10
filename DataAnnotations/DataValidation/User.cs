using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DataValidation;

class User
{
    /*
     * NOTE
     *
     * The property must be public, otherwise it's ineligible for validation by the Validator!
     */
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
