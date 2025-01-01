using System.Text.Json;

namespace LinqOperators;

class Student
{
    public required int ID { get; init; }

    public required int DepartmentID { get; init; }

    public required string Name { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static IEnumerable<Student> Collection => [
        new Student() { ID = 1, DepartmentID = 1, Name = "S1" },
        new Student() { ID = 2, DepartmentID = 1, Name = "S2" },
        new Student() { ID = 3, DepartmentID = 2, Name = "S3" },
        new Student() { ID = 4, DepartmentID = 3, Name = "S4" },
    ];
}
