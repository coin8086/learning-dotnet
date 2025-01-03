using System.Text.Json;

namespace EFCommon.Models;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public int BlogId { get; set; }

    public Blog Blog { get; set; } = default!;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
