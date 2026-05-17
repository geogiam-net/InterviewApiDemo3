
using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using Demo.Domain.Validators;
using Demo.Domain.Exceptions;

namespace Demo.Domain.Services;

public class ShiftService(IShiftRepository shiftRepository, IEmployeeService employeeService) : IShiftService
{
    public async Task<Shift> CreateShiftAsync(string name, Role role, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
    { 
        var newShift = new Shift(name, role, startTime, endTime);

        var errorMessages = ShiftValidator.ValidateDates(newShift);

        if (errorMessages.Any())
        {
            throw new ValidationException(errorMessages);
        }

        return await shiftRepository.CreateShiftAsync(newShift, cancellationToken);
    }

    public async Task<Shift?> GetShiftAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await shiftRepository.GetShiftAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await shiftRepository.GetShiftsFromEmployeeAsync(employeeId, cancellationToken);
    }

    public async Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        return await shiftRepository.GetShiftsOnDateAsync(date, cancellationToken);
    }

    public async Task AssignEmployeeToShiftAsync(Guid shiftId, Guid employeeId, CancellationToken cancellationToken = default)
    {
        var shift = await shiftRepository.GetShiftAsync(shiftId, cancellationToken);
        if (shift is null)
        {
            throw new NotFoundException(["Shift not found"]);
        }

        var employee = await employeeService.GetEmployeeAsync(employeeId, cancellationToken);
        if (employee is null)
        {
            throw new NotFoundException(["Employee not found"]);
        }

        if(shift.Employees.Any(e => e.Id == employeeId))
        {
            return;
        }

        var employeeShifts = await shiftRepository.GetShiftsFromEmployeeAsync(employee.Id, cancellationToken);
        var errorMessages = ShiftValidator.ValidateShiftAssignment(shift, employeeShifts);
        if (errorMessages.Any())
        {
            throw new ConflictException(errorMessages);
        }

        await shiftRepository.AssignEmployeeToShiftAsync(shift.Id, employee.Id, cancellationToken);
    }
}