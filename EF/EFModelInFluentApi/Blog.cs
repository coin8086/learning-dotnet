using Microsoft.EntityFrameworkCore;

namespace EFModelInFluentApi;

[EntityTypeConfiguration(typeof(BlogModelConfig))]
public class Blog
{
    public int BlogId { get; set; }

    public string? Url { get; set; }
}
