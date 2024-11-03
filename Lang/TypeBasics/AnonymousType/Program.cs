namespace AnonymousType;

class Program
{
    static void Main(string[] args)
    {
        var value = new { X = 1, Y = "Hello" };
        Console.WriteLine(value.ToString());
        Console.WriteLine(value.GetType().Name);

        var value2 = new { X = 1, Y = "" };
        Console.WriteLine(value2.ToString());
        Console.WriteLine(value2.GetType().Name);
        Console.WriteLine(value == value2);

        var value3 = new { X = 1, Y = "Hello" };
        Console.WriteLine(value3.ToString());
        Console.WriteLine(value3.GetType().Name);
        Console.WriteLine(value == value3);
    }
}
