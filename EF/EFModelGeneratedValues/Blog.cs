using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace EFModelGeneratedValues;

public class Blog
{
    public int Id { get; set; }

    public string? Name { get; set; }

    //Computed property from DB. Will never be null.
    public string? InternalName { get; }

    //The SQLite provider doesn't support Identity.
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; }

    //The SQLite provider doesn't support Computed.
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
