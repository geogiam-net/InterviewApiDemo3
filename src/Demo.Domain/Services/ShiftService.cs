
using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using Demo.Domain.Validators;
using Demo.Domain.Exceptions;

namespace Demo.Domain.Services;

public class ShiftService(IShiftStorageService shiftStorageService, IEmployeeService employeeService) : IShiftService
{
    public async Task<Shift> CreateShiftAsync(string name, Role role, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
    { 
        var newShift = new Shift(name, role, startTime, endTime);

        var errorMessages = ShiftValidator.ValidateDates(newShift);

        if (errorMessages.Any())
        {
            throw new ValidationException(errorMessages);
        }

        return await shiftStorageService.CreateShiftAsync(newShift, cancellationToken);
    }

    public async Task<Shift?> GetShiftAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await shiftStorageService.GetShiftAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await shiftStorageService.GetShiftsFromEmployeeAsync(employeeId, cancellationToken);
    }

    public async Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        return await shiftStorageService.GetShiftsOnDateAsync(date, cancellationToken);
    }

    public async Task AssignShiftToEmployeeAsync(Guid shiftId, Guid employeeId, CancellationToken cancellationToken = default)
    {
        var shift = await shiftStorageService.GetShiftAsync(shiftId, cancellationToken);
        if (shift is null)
        {
            throw new ValidationException(["Shift not found"]);
        }

        var employee = await employeeService.GetEmployeeAsync(employeeId, cancellationToken);
        if (employee is null)
        {
            throw new ValidationException(["Employee not found"]);
        }

        await AssignShiftToEmployeeAsync(shift, employee, cancellationToken);
    }

    public async Task AssignShiftToEmployeeAsync(Shift shift, Employee employee, CancellationToken cancellationToken = default)
    {
        var employeeShifts = await shiftStorageService.GetShiftsFromEmployeeAsync(employee.Id, cancellationToken);

        var errorMessages = ShiftValidator.ValidateShiftAssignment(shift, employeeShifts);
        if (errorMessages.Any())
        {
            throw new ValidationException(errorMessages);
        }

        await shiftStorageService.AssignShiftToEmployeeAsync(shift, employee, cancellationToken);
    }
}