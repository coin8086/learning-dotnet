namespace EFModelNavigation.Models;

public class Blog2
{
    public int Id { get; set; }

    /*
     * "Even though the collection instance must be an ICollection<T>, the collection
     * does not need to be exposed as such."
     */
    public IEnumerable<Post> Posts { get; } = new List<Post>();
}
