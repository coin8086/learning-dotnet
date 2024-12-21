namespace EFModelRelationships.Models;

public class Employee
{
    public int Id { get; set; }

    public int? ManagerId { get; set; }

    public Employee? Manager { get; set; }

    public IList<Employee> Reports { get; set; } = new List<Employee>();
}
