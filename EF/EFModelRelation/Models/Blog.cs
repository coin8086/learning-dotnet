namespace EFModelRelation.Models;

public class Blog
{
    public int Id { get; set; }

    public IList<Post> Posts { get; } = new List<Post>();
}
