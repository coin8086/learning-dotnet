//See https://learn.microsoft.com/en-us/ef/core/modeling/

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFModelInFluentApi;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();
        
        Console.WriteLine($"DB path: {db.DbPath}.");

        //Short form
        //Console.WriteLine(db.Model.ToDebugString());

        //Long form
        Console.WriteLine(db.Model.ToDebugString(MetadataDebugStringOptions.LongDefault));
    }
}
