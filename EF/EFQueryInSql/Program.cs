//See https://learn.microsoft.com/en-us/ef/core/querying/sql-queries?tabs=sqlite

using EFCommon;
using EFCommon.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;

namespace EFQueryInSql;

class Program
{
    static CommonDbContext CreateContext()
    {
        return new SqliteContext(nameof(EFQueryInSql));
        //return new PostgreSqlContext(nameof(EFQueryInSql), "Host=***;Port=5432;Username=***;Password=***", "10.17");
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
         * 1) Here $"..." is different than "..."! The latter doesn't pass the compiler! See more at
         * https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.relationalqueryableextensions.fromsql
         *
         * 2) For PostgreSQL, the table name must be double quoted and the name is case-sensitive,
         * while for SQLite, the table name doesn't require quoting and the name is not case-sensitive.
         */
        var query = context.Blogs.FromSql($""" select * from "Blogs" """);

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void BasicQueryWithParameter()
    {
        using var context = CreateContext();
        var id = 1;
        /*
         * NOTE
         *
         *  Here the same notes apply to the column name "Id" in query, as those for the the table name in BasicQuery.
         */
        var query = context.Blogs.FromSql($""" select * from "Blogs" where "Id" = {id} """);

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
         * A provider-specific type is required for parameter.
         */
        DbParameter columnValue;

        if (context is SqliteContext)
        {
            columnValue = new SqliteParameter("columnValue", 1);
        }
        else if (context is PostgreSqlContext)
        {
            columnValue = new NpgsqlParameter("columnValue", 1);
        }
        else
        {
            throw new NotSupportedException();
        }

#pragma warning disable EF1002
        //FromSqlRaw will trigger warning EF1002. Disable it here.
        var query = context.Blogs.FromSqlRaw($""" select * from "Blogs" where "{columnName}" = @columnValue """, columnValue);
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
