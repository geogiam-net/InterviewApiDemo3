using Microsoft.EntityFrameworkCore;
using Demo.Business.Extensions;
using Demo.Business.Interfaces;
using Demo.Business.Models;
using Demo.Infrastructure.SqlStorage.Data;

namespace Demo.Infrastructure.SqlStorage.Services;

public class ShiftRepository(ApplicationDbContext dbContext) : IShiftRepository
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
            .Include(s => s.Employees)
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
            .Include(s => s.Employees)
            .Where(s => dateTime.IsInsideDateRange(s.StartTime, s.EndTime))
            .ToListAsync(cancellationToken);
    }

    public async Task AssignEmployeeToShiftAsync(Guid shiftId, Guid employeeId, CancellationToken cancellationToken = default)
    {
        // Load the tracked shift including its employees from the context
        var trackedShift = await dbContext.Shifts
            .Include(s => s.Employees)
            .FirstOrDefaultAsync(s => s.Id == shiftId, cancellationToken);

        // Load tracked Employee instance so EF can manage the relationship correctly
        var trackedEmployee = await dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);

        if(trackedShift is null || trackedEmployee is null)
        {
            return;
        }

        trackedShift.Employees.Add(trackedEmployee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
