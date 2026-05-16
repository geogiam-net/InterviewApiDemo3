using Demo.Domain.Models;

namespace Demo.Api.Dtos;

public class ShiftDto
{
    public Guid Id { get; set; } = default;

    public string Name { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.None;

    public DateTime StartTime { get; set; } = default;

    public DateTime EndTime { get; set; } = default;

    public List<Employee> Employees { get; set; } = new List<Employee>();


    public ShiftDto()
    {
    }

    public ShiftDto(Shift shift)
    {
        Id = shift.Id;
        Name = shift.Name;
        Role = shift.Role;
        StartTime = shift.StartTime;
        EndTime = shift.EndTime;
        Employees = shift.Employees;
    }
}
