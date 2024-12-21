using Microsoft.EntityFrameworkCore;
using EFMigration.Models;

namespace EFMigration.Data;

public class SqliteContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbFilePath { get; }

    public SqliteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbFilePath = Path.Join(path, $"{nameof(EFMigration)}.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbFilePath}");
    }
}
