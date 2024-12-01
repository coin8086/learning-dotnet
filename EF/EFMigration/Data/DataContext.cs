using Microsoft.EntityFrameworkCore;
using EFMigration.Models;

namespace EFMigration.Data;

public class DataContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbPath { get; }

    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "blogging2.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
