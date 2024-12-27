namespace EFModelNavigation.Models;

public class Blog
{
    public int Id { get; set; }

    /*
     * "The underlying collection instance must implement ICollection<T>, and must have
     * a working Add method."
     */
    public IList<Post> Posts { get; } = new List<Post>();
}
