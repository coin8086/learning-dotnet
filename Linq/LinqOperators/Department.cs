using System.Text.Json;

namespace LinqOperators;

class Department
{
    public int ID { get; init; }

    public required string Name { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static IEnumerable<Department> Collection => [
        new Department() { ID = 1, Name= "D1" },
        new Department() { ID = 2, Name= "D2" },
    ];
}
