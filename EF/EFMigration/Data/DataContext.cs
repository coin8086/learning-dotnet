using Microsoft.EntityFrameworkCore;
using EFMigration.Models;

namespace EFMigration.Data;

public class DataContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbFilePath { get; }

    public DataContext()
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
