using Demo.Domain.Models;

namespace Demo.Domain.Interfaces;

public interface IShiftStorageService
{
    public Task<Shift> CreateShiftAsync(Shift shift, CancellationToken cancellationToken = default);

    public Task<Shift?> GetShiftAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken cancellationToken = default);

    public Task AssignEmployeeToShiftAsync(Guid shiftId, Guid employeeId, CancellationToken cancellationToken = default);
}