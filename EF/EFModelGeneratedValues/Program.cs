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
        Console.WriteLine(blogEntry);
        Console.WriteLine(blog);

        db.SaveChanges();

        Console.WriteLine("----------------");
        Console.WriteLine(blogEntry);

        var blog2 = db.Blogs.Where(b => b.Id == blogEntry.Entity.Id).First();
        Console.WriteLine(blog2);
    }
}
