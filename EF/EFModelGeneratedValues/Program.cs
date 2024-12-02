//See https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations

using EFModelGeneratedValues.Data;
using EFModelGeneratedValues.Models;

namespace EFModelGeneratedValues;

class Program
{
    static void Main(string[] args)
    {
        using var db = new DataContext();
        Console.WriteLine($"DB file: {db.DbFilePath}");

        //db.Database.EnsureDeleted();
        //db.Database.EnsureCreated();

        var ts = DateTime.Now;
        //NOTE: For the SQLite provider, CreatedAt and UpdatedAt have to be set manually.
        var blog = new Blog() { Name = "Blog 1", CreatedAt = ts, UpdatedAt = ts };
        var blogEntity = db.Add(blog);
        Console.WriteLine(blogEntity);

        db.SaveChanges();

        Console.WriteLine("----------------");
        Console.WriteLine(blogEntity);
    }
}
