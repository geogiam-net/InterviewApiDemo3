using Demo.Application.Dtos;
using Demo.Application.Interfaces;
using Demo.Domain.Enums;
using Demo.Domain.Models;
using Demo.Domain.Validators;

namespace Demo.Application.Services;

public class ShiftService(IShiftRepository shiftRepository, IEmployeeService employeeService) : IShiftService
{
    public async Task<ResultDto<Shift?>> CreateShiftAsync(
        string name, 
        Role role, 
        DateTime startTime, 
        DateTime endTime, 
        CancellationToken ct)
    { 
        var newShift = new Shift(name, role, startTime, endTime);

        var errorMessages = ShiftValidator.ValidateDates(newShift);

        if (errorMessages.Any())
        {
            return new ResultDto<Shift?>(null, Error.ValidationError, errorMessages);
        }

        return new ResultDto<Shift?>(await shiftRepository.CreateShiftAsync(newShift, ct));
    }

    public async Task<ResultDto<Shift?>> GetShiftAsync(Guid id, CancellationToken ct)
    {
        var shift = await shiftRepository.GetShiftAsync(id, ct);
        if (shift is null)
        {
            return new ResultDto<Shift?>(null, Error.NotFound, new List<string> { "Shift not found" });
        }

        return new ResultDto<Shift?>(shift);
    }

    public async Task<IEnumerable<Shift>> GetShiftsFromEmployeeAsync(Guid employeeId, CancellationToken ct)
    {
        return await shiftRepository.GetShiftsFromEmployeeAsync(employeeId, ct);
    }

    public async Task<IEnumerable<Shift>> GetShiftsOnDateAsync(DateOnly date, CancellationToken ct)
    {
        return await shiftRepository.GetShiftsOnDateAsync(date, ct);
    }

    public async Task<ResultDto<bool>> AssignEmployeeToShiftAsync(Guid shiftId, Guid employeeId, CancellationToken ct)
    {
        var shift = await shiftRepository.GetShiftAsync(shiftId, ct);
        if (shift is null)
        {
            return new ResultDto<bool>(false, Error.NotFound, new List<string> { "Shift not found" });
        }

        var employeeResult = await employeeService.GetEmployeeAsync(employeeId, ct);
        if (employeeResult.ErrorCode != Error.None)
        {
            return new ResultDto<bool>(false, employeeResult.ErrorCode, employeeResult.ErrorMessages);
        }

        if(shift.Employees.Any(e => e.Id == employeeId))
        {
            return new ResultDto<bool>(false, Error.Conflict, new List<string> { "Employee is already assigned to this shift" });

        }

        var employeeShifts = await shiftRepository.GetShiftsFromEmployeeAsync(employeeResult.Result!.Id, ct);
        var errorMessages = ShiftValidator.ValidateShiftAssignment(shift, employeeShifts);
        if (errorMessages.Any())
        {
            return new ResultDto<bool>(false, Error.Conflict, errorMessages);
        }

        await shiftRepository.AssignEmployeeToShiftAsync(shift.Id, employeeResult.Result.Id, ct);
        return new ResultDto<bool>(true);
    }
}