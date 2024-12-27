//See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
//and https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli

using EFMigration.Data;

namespace EFMigration;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();
        Console.WriteLine(db);
    }
}
