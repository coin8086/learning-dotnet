//See https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations

namespace EFModelGeneratedValues;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();
        Console.WriteLine($"DB file: {db.DbFilePath}");

        //var ts = DateTime.Now;
        var blog = new Blog() { /* CreatedAt = ts, UpdatedAt = ts */};
        var blogEntry = db.Add(blog);

        Console.WriteLine("Before save:");
        Console.WriteLine(blog);
        Console.WriteLine(blogEntry);

        db.SaveChanges();

        Console.WriteLine("After save:");
        Console.WriteLine(blog);
        Console.WriteLine(blogEntry);

        Console.WriteLine("----------------");

        blog.Name = "A blog";

        Console.WriteLine("Before save:");
        Console.WriteLine(blog);
        Console.WriteLine(blogEntry);

        db.SaveChanges();

        Console.WriteLine("After save:");
        Console.WriteLine(blog);
        Console.WriteLine(blogEntry);
    }
}
