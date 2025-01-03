using System.Text.Json;

namespace EFCommon.Models;

public class Blog
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public IList<Post> Posts { get; set;  } = new List<Post>();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
