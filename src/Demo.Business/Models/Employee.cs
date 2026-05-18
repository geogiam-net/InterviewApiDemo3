namespace Demo.Business.Models;

public class Employee
{
    public Employee() { }

    public Employee(string name, string lastname)
    {
        Name = name;
        Lastname = lastname;
    }

    public Guid Id { get; set; } = default;

    public string Name { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;
}
