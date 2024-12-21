using EFModelRelation.Models;
using Microsoft.EntityFrameworkCore;

namespace EFModelRelation;

public class SqliteContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public string DbPath { get; }

    public SqliteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, $"{nameof(EFModelRelation)}.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
