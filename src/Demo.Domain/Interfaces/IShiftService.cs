using Demo.Domain.Models;

namespace Demo.Domain.Interfaces;

public interface IShiftService
{
    public Task<Shift> CreateShiftAsync(string name, Role role, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

    public Task<Shift?> GetShiftAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken cancellationToken = default);

    public Task AssignShiftToEmployeeAsync(Guid shiftId, Guid employeeId, CancellationToken cancellationToken = default);

    public Task AssignShiftToEmployeeAsync(Shift shift, Employee employee, CancellationToken cancellationToken = default);
}