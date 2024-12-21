using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFModelBackingFields;

public class Blog2EntityConfig : IEntityTypeConfiguration<Blog2>
{
    public void Configure(EntityTypeBuilder<Blog2> builder)
    {
        builder.Property(e => e.Url).HasField("_validatedUrl");
    }
}
