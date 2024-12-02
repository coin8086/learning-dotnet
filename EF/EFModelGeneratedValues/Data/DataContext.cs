using EFModelGeneratedValues.Models;
using Microsoft.EntityFrameworkCore;

namespace EFModelGeneratedValues.Data;

public class DataContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbFilePath { get; }

    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbFilePath = Path.Join(path, $"{nameof(EFModelGeneratedValues)}.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbFilePath}");
    }
}
