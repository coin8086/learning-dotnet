using System.Reflection;
using System.Text.Json;

namespace InterfaceSerialization;

interface IPerson
{
    string Name { get; set; }

    string Property { get; set; }
}

class Person : IPerson
{
    public string Name { get; set; } = default!;

    public int Age { get; set; }
    
    public int Property { get; set; }

    string IPerson.Property 
    { 
        get => Property.ToString();

        set
        {
            Property = int.Parse(value);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var person = new Person() { Name = "Bob", Age = 5, Property = 100 };
        IPerson iperson = person;
        Console.WriteLine(JsonSerializer.Serialize(person));
        Console.WriteLine(JsonSerializer.Serialize(iperson));
    }
}
