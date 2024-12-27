//See https://learn.microsoft.com/en-us/ef/core/modeling/

namespace EFModelInFluentApi;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();
        Console.WriteLine(db);
    }
}
