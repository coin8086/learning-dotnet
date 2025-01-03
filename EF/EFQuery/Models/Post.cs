using System.Text.Json;

namespace EFQuery.Models;

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
