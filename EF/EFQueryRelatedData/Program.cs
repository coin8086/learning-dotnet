//See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
//and https://learn.microsoft.com/en-us/ef/core/querying/related-data/explicit
//and https://learn.microsoft.com/en-us/ef/core/querying/related-data/serialization

using EFCommon.Models;
using EFCommon;
using Microsoft.EntityFrameworkCore;

namespace EFQueryRelatedData;

class Program
{
    static bool _usePostgreSQL = false;

    static CommonDbContext CreateContext()
    {
        var dbName = nameof(EFQueryRelatedData);
        return _usePostgreSQL ? new PostgreSqlContext(dbName) : new SqliteContext(dbName);
    }

    static void InitDb()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var blog1 = new Blog()
        {
            Name = "blog1",
            Posts = [
                new Post() { Title = "post1" },
                new Post() { Title = "post2" },
            ]
        };
        var blog2 = new Blog()
        {
            Name = "blog2",
            Posts = [
                new Post() { Title = "post3" },
            ]
        };
        var blog3 = new Blog() { Name = "blog3" };

        context.Blogs.AddRange(blog1, blog2, blog3);
        context.SaveChanges();
    }

    static void Query()
    {
        using var context = CreateContext();
        var query = context.Blogs;

        foreach (var blog in query)
        {
            Console.WriteLine(blog);
        }
    }

    static void QueryWithEagerLoading()
    {
        using var context = CreateContext();
        var query = context.Blogs.Include(blog => blog.Posts);

        foreach (var blog in query)
        {
            Console.WriteLine(blog);
        }
    }

    static void ExplicitLoading()
    {
        using var context = CreateContext();
        var blog = context.Blogs.Single(blog => blog.Name == "blog1");

        Console.WriteLine($"Before: {blog}");

        context.Entry(blog).Collection(blog => blog.Posts).Load();

        Console.WriteLine($"After: {blog}");
    }

    static void ExplicitLoadingWithChainedQuery()
    {
        using var context = CreateContext();
        var blog = context.Blogs.Single(blog => blog.Name == "blog1");

        Console.WriteLine($"Before: {blog}");

        context.Entry(blog).Collection(blog => blog.Posts).Query().Where(post => post.Title != "post1").Load();

        Console.WriteLine($"After: {blog}");
    }

    static void Main(string[] args)
    {
        if (args.Length > 0 && "-p".Equals(args[0], StringComparison.Ordinal))
        {
            _usePostgreSQL = true;
        }

        InitDb();

        Console.WriteLine($"\n----------- {nameof(Query)} -----------");
        Query();

        Console.WriteLine($"\n----------- {nameof(QueryWithEagerLoading)} -----------");
        QueryWithEagerLoading();

        Console.WriteLine($"\n----------- {nameof(ExplicitLoading)} -----------");
        ExplicitLoading();

        Console.WriteLine($"\n----------- {nameof(ExplicitLoadingWithChainedQuery)} -----------");
        ExplicitLoadingWithChainedQuery();
    }
}
