using Microsoft.EntityFrameworkCore;

namespace EFModelInFluentApi;

public class SqliteContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbPath { get; }

    public SqliteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, $"{nameof(EFModelInFluentApi)}.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //Configure conventions like
        //configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // To configure the Blog entity type, both the following ways are OK.
        //
        // modelBuilder.Entity<Blog>().Property(b => b.Url).IsRequired();
        //
        // new BlogModelConfig().Configure(modelBuilder.Entity<Blog>());
        //
        // Also note that any entities or entity properties that are configured here
        // will also be added to the model if they're not already.
    }
}
