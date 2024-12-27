//See https://learn.microsoft.com/en-us/ef/core/modeling/backing-field?tabs=data-annotations

namespace EFModelBackingFields;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();
        Console.WriteLine(db);
    }
}
