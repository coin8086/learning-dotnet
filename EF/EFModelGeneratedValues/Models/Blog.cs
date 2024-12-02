using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace EFModelGeneratedValues.Models;

public class Blog
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    //NOTE: The there's a bug on the value "Computed" for the SQLite provider.
    //Change it to "Identity" for a workaround.
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
