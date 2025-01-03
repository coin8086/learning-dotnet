//See https://learn.microsoft.com/en-us/ef/core/querying/sql-queries?tabs=sqlite

using EFCommon;
using EFCommon.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFQueryInSql;

class Program
{
    //TODO: PostgreSqlContext produces runtime error 42P01 for queries. Fix it.
    static CommonDbContext CreateContext()
    {
        return new SqliteContext(nameof(EFQueryInSql));
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

    static void BasicQuery()
    {
        using var context = CreateContext();
        /*
         * NOTE
         * 
         * Here $"..." is different than "..."! The latter doesn't pass the compiler! See more at
         * https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.relationalqueryableextensions.fromsql
         */
        var query = context.Blogs.FromSql($"select * from blogs");

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void BasicQueryWithParameter()
    {
        using var context = CreateContext();
        var id = 1;
        var query = context.Blogs.FromSql($"select * from blogs where Id = {id}");

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void QueryInRawSql()
    {
        using var context = CreateContext();
        var columnName = "Id";
        /*
         * NOTE
         *
         * Here a provider-specific type SqliteParameter is required.
         */
        var columnValue = new SqliteParameter("columnValue", 1);

#pragma warning disable EF1002
        //FromSqlRaw will trigger warning EF1002. Disable it here.
        var query = context.Blogs.FromSqlRaw($"select * from blogs where {columnName} = @columnValue", columnValue);
#pragma warning restore

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void Main(string[] args)
    {
        InitDb();

        Console.WriteLine($"\n---------------- {nameof(BasicQuery)} ----------------");
        BasicQuery();

        Console.WriteLine($"\n---------------- {nameof(BasicQueryWithParameter)} ----------------");
        BasicQueryWithParameter();

        Console.WriteLine($"\n---------------- {nameof(QueryInRawSql)} ----------------");
        QueryInRawSql();
    }
}
