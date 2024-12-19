//See https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
//and https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/
//and https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/
//and https://www.nuget.org/packages/dotnet-ef

namespace EFGetStarted;

class Program
{
    static void Main(string[] args)
    {
        using var db = new BloggingContext();
        Console.WriteLine($"Database path: {db.DbPath}.");

        // Create
        Console.WriteLine("Inserting a new blog");
        db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a blog");
        var blog = db.Blogs
            .OrderBy(b => b.BlogId)
            .First();

        Console.WriteLine($"Retrieved blog: {blog}");

        // Update
        Console.WriteLine("Updating the blog and adding posts");
        blog.Url = "https://devblogs.microsoft.com/dotnet";
        blog.Posts.AddRange([
            new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" },
            new Post { Title = "Hello World2", Content = "I wrote an app using EF Core, too!" },
        ]);
        db.SaveChanges();

        var posts = db.Posts.OrderBy(b => b.PostId);
        foreach (var post in posts)
        {
            Console.WriteLine($"Retrieved post: {post}");
        }

        // Delete
        Console.WriteLine("Delete the blog and posts");
        Console.Write("Press any key to continue...");
        Console.ReadKey(false);

        db.Remove(blog);

        //Posts that belong to the blog will be cascade-deleted so this is unnecessary.
        //db.RemoveRange(posts);

        db.SaveChanges();
    }
}
