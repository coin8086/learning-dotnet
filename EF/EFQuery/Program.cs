//See https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators

using EFCommon;
using EFCommon.Models;

namespace EFQuery;

class Program
{
    static SqliteContext CreateContext()
    {
        return new SqliteContext(nameof(EFQuery));
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

    static void InnerJoinByJoin()
    {
        using var context = CreateContext();
        var query = context.Blogs.Join(
            context.Posts,
            blog => blog.Id,
            post => post.BlogId,
            (blog, post) => new { BlogName = blog.Name, PostTitle = post.Title });

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void InnerJoinBySelectMany()
    {
        using var context = CreateContext();
        var query = context.Blogs.SelectMany(
            blog => blog.Posts,
            (blog, post) => new { BlogName = blog.Name, PostTitle = post.Title });

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void LeftJoinByGroupJoinAndSelectMany()
    {
        using var context = CreateContext();
        var query =
            context.Blogs.GroupJoin(
                context.Posts,
                blog => blog.Id,
                post => post.BlogId,
                (blog, posts) => new { BlogName = blog.Name, Posts = posts })
            .SelectMany(
                item => item.Posts.DefaultIfEmpty(),
                /*
                 * NOTE
                 *
                 * The following modern expression is disallowed and will produce error CS8072 -
                 * "An expression tree lambda may not contain a null propagating operator."
                 * 
                 * PostTitle = post?.Title ?? "(null)"
                 * 
                 * "All of the errors in the preceding list indicate you've used a C# expression type
                 * that isn't allowed in an expression tree. In most cases, the prohibited expressions
                 * represent syntax introduced after C# 3.0. These expressions are prohibited because
                 * allowing them would create a breaking change in all libraries that parse expression trees."
                 *
                 * See more at
                 * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/expression-tree-restrictions
                 */
                (item, post) => new { BlogName = item.BlogName, PostTitle = (post == null ? "(null)" : post.Title) });

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void LeftJoinBySelectMany()
    {
        using var context = CreateContext();
        var query = context.Blogs.SelectMany(
            blog => blog.Posts.DefaultIfEmpty(),
            (blog, post) => new { BlogName = blog.Name, PostTitle = (post == null ? "(null)" : post.Title) });

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void Group()
    {
        using var context = CreateContext();

        /*
         * NOTE
         *
         * 1) The statement
         *
         * context.Posts.GroupBy(post => post.BlogId).OrderBy(group => group.Key);
         *
         * will produce a runtime error
         *
         * System.InvalidOperationException: The LINQ expression '...' could not be translated.
         * Either rewrite the query in a form that can be translated, or switch to client evaluation
         * explicitly by inserting a call to 'AsEnumerable', 'AsAsyncEnumerable', 'ToList', or
         * 'ToListAsync'. See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.
         *
         * 2) Even without "OrderBy" call, the statement
         *
         * context.Posts.GroupBy(post => post.BlogId);
         *
         * still depends on client evaluation for grouping by EF Core. No "group by" SQL will be generated.
         *
         * 3) To generate SQL "group by", an aggregation function like Count must be included, and no column
         * other than the group Key(s) can be selected. See more at
         * https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators#groupby
         */
        var query = context.Posts.GroupBy(post => post.BlogId).OrderBy(group => group.Key)
            .Select(group => new { BlogId = group.Key, Count = group.Count() });

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void Main(string[] args)
    {
        InitDb();

        Console.WriteLine($"\n----------- {nameof(InnerJoinByJoin)} -----------");
        InnerJoinByJoin();

        Console.WriteLine($"\n----------- {nameof(InnerJoinBySelectMany)} -----------");
        InnerJoinBySelectMany();

        Console.WriteLine($"\n----------- {nameof(LeftJoinByGroupJoinAndSelectMany)} -----------");
        LeftJoinByGroupJoinAndSelectMany();

        Console.WriteLine($"\n----------- {nameof(LeftJoinBySelectMany)} -----------");
        LeftJoinBySelectMany();

        Console.WriteLine($"\n----------- {nameof(Group)} -----------");
        Group();
    }
}
