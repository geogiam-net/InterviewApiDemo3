using Microsoft.EntityFrameworkCore;
using Demo.Domain.Extensions;
using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using Demo.Infrastructure.SqlStorage.Data;

namespace Demo.Infrastructure.SqlStorage.Services;

public class ShiftStorageService(ApplicationDbContext dbContext) : IShiftStorageService
{
    public async Task<Shift> CreateShiftAsync(Shift shift, CancellationToken cancellationToken = default)
    {
        dbContext.Shifts.Add(shift);
        await dbContext.SaveChangesAsync(cancellationToken);
        return shift;
    }

    public async Task<Shift?> GetShiftAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Shifts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Shifts
            .AsNoTracking()
            .Include(s => s.Employees)
            .Where(s => s.Employees.Any(e => e.Id == employeeId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        var dateTime = date.ToDateTime(TimeOnly.MinValue);

        return await dbContext.Shifts
            .AsNoTracking()
            .Where(s => dateTime.IsInsideTimeRange(s.StartTime, s.EndTime))
            .ToListAsync(cancellationToken);
    }

    public async Task AssignShiftToEmployeeAsync(Shift shift, Employee employee, CancellationToken cancellationToken = default)
    {
        shift.Employees.Add(employee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
