using Microsoft.EntityFrameworkCore;

namespace EFModelGeneratedValues;

public class SqliteContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbFilePath { get; }

    public SqliteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbFilePath = Path.Join(path, $"{nameof(EFModelGeneratedValues)}.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbFilePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>().Property(b => b.Name).HasDefaultValue("Default Blog Name");

        /*
         * NOTE
         *
         * The "||" operator is SQLite specific.
         */
        modelBuilder.Entity<Blog>().Property(b => b.InternalName).HasComputedColumnSql("[Id] || '-' || [Name]", true);

        /*
         * NOTE
         *
         * This generates the following DB schema by the SQLite provider.
         *
         * "InternalName" AS (concat(Id, '-', Name)) STORED
         *
         * But SQLite doesn't accept it!
         */
        //modelBuilder.Entity<Blog>().Property(b => b.InternalName).HasComputedColumnSql("concat(Id, '-', Name)", true);

        /*
         * NOTE
         *
         * This generates the following DB schema in SQLite
         *
         * "CreatedAt" TEXT DEFAULT (datetime('subsec'))
         *
         * The schema works when inserting a row directly in DB by SQLite console. But it doesn't work in the SQLite provider!
         */
        modelBuilder.Entity<Blog>().Property(b => b.CreatedAt).HasDefaultValueSql("datetime('subsec')");
    }
}
