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
    static bool _usePostgreSQL = false;

    static CommonDbContext CreateContext()
    {
        return _usePostgreSQL ? new PostgreSqlContext(nameof(EFQueryInSql)) : new SqliteContext(nameof(EFQueryInSql));
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
         * 2) For PostgreSQL, an identifier like a table/field name must be double quoted to preserve its case, otherwise it
         * will be folded into lowercase and thus produce a runtime error for something is not found by the identifier. See
         * more at https://stackoverflow.com/questions/43111996/why-postgresql-does-not-like-uppercase-table-names
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
        if (args.Length > 0 && "-p".Equals(args[0], StringComparison.Ordinal))
        {
            _usePostgreSQL = true;
        }

        InitDb();

        Console.WriteLine($"\n---------------- {nameof(BasicQuery)} ----------------");
        BasicQuery();

        Console.WriteLine($"\n---------------- {nameof(BasicQueryWithParameter)} ----------------");
        BasicQueryWithParameter();

        Console.WriteLine($"\n---------------- {nameof(QueryInRawSql)} ----------------");
        QueryInRawSql();
    }
}
