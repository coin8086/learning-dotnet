using System.Text.Json;
using System.Text.Json.Serialization;

namespace EFCommon.Models;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public int BlogId { get; set; }

    [JsonIgnore]
    public Blog Blog { get; set; } = default!;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
