//See https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations

namespace EFModelNavigation;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();
        Console.WriteLine(db);
    }
}
