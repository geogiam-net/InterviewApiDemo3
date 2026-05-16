namespace Demo.Domain.Models;

public class Shift
{
    public Shift() { }

    public Shift(string name, Role role, DateTime startTime, DateTime endTime)
    {
        Name = name;
        Role = role;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Guid Id { get; set; } = default;

    public string Name { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.None;

    public DateTime StartTime { get; set; } = default;

    public DateTime EndTime { get; set; } = default;

    public List<Employee> Employees { get; set; } = new List<Employee>();
}
