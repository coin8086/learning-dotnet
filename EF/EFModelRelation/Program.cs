//See https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many
//and https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFModelRelation;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();

        Console.WriteLine($"DB path: {db.DbPath}.");
        Console.WriteLine(db.Model.ToDebugString(MetadataDebugStringOptions.LongDefault));
    }
}
