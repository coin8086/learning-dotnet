namespace EFQuery.Models;

public class Blog
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public IList<Post> Posts { get; set;  } = new List<Post>();
}
