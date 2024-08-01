namespace TypeOf;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(typeof(Program));
        Console.WriteLine(typeof(string));
        //NOTE: This doesn't pass compiler.
        //Console.WriteLine(typeof(string?));
    }
}
