using System.Text.Json.Serialization;
using System.Text.Json;

namespace TupleInJson;

class SomeClass
{
    public string? Property1 { get; set; }

    public int Property2 { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        var options = new JsonSerializerOptions
        {
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            //IncludeFields = true,
        };

        /*
         * NOTE
         * 
         * A tuple defined like this requires option IncludeFields = true to work in JSON serialization. Try it.
         * 
         * var tuple = (new SomeClass() { Property1 = "Hello", Property2 = 100 }, 1, 2);
         */
        var tuple = new Tuple<SomeClass, int, int>(new SomeClass() { Property1 = "Hello", Property2 = 100 }, 1, 2);

        var jsonString = JsonSerializer.Serialize(tuple, options);
        Console.WriteLine(jsonString);

        Console.WriteLine("---------------");

        var tupleBack = JsonSerializer.Deserialize<Tuple<SomeClass, int, int>>(jsonString, options);
        Console.WriteLine(tupleBack);
    }
}
