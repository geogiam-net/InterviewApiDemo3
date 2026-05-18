using Demo.Business.Models;

namespace Demo.Business.Interfaces;

public interface IShiftRepository
{
    public Task<Shift> CreateShiftAsync(Shift shift, CancellationToken cancellationToken = default);

    public Task<Shift?> GetShiftAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken cancellationToken = default);

    public Task AssignEmployeeToShiftAsync(Guid shiftId, Guid employeeId, CancellationToken cancellationToken = default);
}