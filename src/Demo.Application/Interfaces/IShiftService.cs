using Demo.Application.Dtos;
using Demo.Domain.Enums;
using Demo.Domain.Models;

namespace Demo.Application.Interfaces;

public interface IShiftService
{
    public Task<ResultDto<Shift?>> CreateShiftAsync(
        string name, 
        Role role, 
        DateTime startTime, 
        DateTime endTime, 
        CancellationToken ct);

    public Task<ResultDto<Shift?>> GetShiftAsync(Guid id, CancellationToken ct);

    public Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken ct);

    public Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken ct);

    public Task<ResultDto<bool>> AssignEmployeeToShiftAsync(Guid shiftId, Guid employeeId, CancellationToken ct);
}