using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFModelBackingFields;

public class Blog3EntityConfig : IEntityTypeConfiguration<Blog3>
{
    public void Configure(EntityTypeBuilder<Blog3> builder)
    {
        builder.Property("_validatedUrl").HasColumnName("Url").IsRequired();
    }
}
