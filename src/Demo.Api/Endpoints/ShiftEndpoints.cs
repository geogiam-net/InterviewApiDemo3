using Demo.Api.Dtos;
using Demo.Application.Interfaces;
using Demo.Domain.Enums;

namespace Demo.Api.Endpoints;

internal static class ShiftEndpoints
{
    internal static void MapShiftEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Routes.Shifts, CreateShiftAsync);
        builder.MapGet(Routes.ShiftById, ShiftByIdAsync);
        builder.MapGet(Routes.ShiftsFromEmployee, GetShiftsFromEmployeeAsync);
        builder.MapGet(Routes.ShiftsOnDate, GetShiftsOnDateAsync);
        builder.MapPut(Routes.AssignEmployeeToShift, AssignEmployeeToShiftAsync);
    }

    private static async Task<IResult> CreateShiftAsync(
        NewShiftDto shift,
        IShiftService shiftService,
        CancellationToken ct
    )
    {
        var result = await shiftService.CreateShiftAsync(shift.Name, shift.Role, shift.StartTimeUtc, shift.EndTimeUtc, ct);

        if (result.ErrorCode == Error.ValidationError)
        {
            return Results.BadRequest(result.ErrorMessages);
        }
        else if (result.ErrorCode == Error.Conflict)
        {
            return Results.Conflict(result.ErrorMessages);
        }
        else if (result.ErrorCode != Error.None)
        {
            return Results.InternalServerError(result.ErrorMessages);
        }

        // return 201 with link to created resource
        return TypedResults.Created(
                  uri: $"{Routes.Shifts}/{result.Result!.Id}",
                  value: new ShiftDto(result.Result));
    }

    private static async Task<IResult> ShiftByIdAsync(
        Guid id,
        IShiftService shiftService,
        CancellationToken ct
    )
    {
        var result = await shiftService.GetShiftAsync(id, ct);

        if (result.ErrorCode == Error.NotFound)
        {
            return Results.NotFound(result.ErrorMessages);
        }
        else if (result.ErrorCode != Error.None)
        {
            return Results.InternalServerError(result.ErrorMessages);
        }

        return TypedResults.Ok(new ShiftDto(result.Result!));
    }

    private static async Task<IResult> GetShiftsFromEmployeeAsync(
        Guid employeeId,
        IShiftService shiftService,
        CancellationToken ct
    )
    {
        var shifts = await shiftService.GetShiftsFromEmployeeAsync(employeeId, ct);
        if (shifts == null || !shifts.Any())
        {
            return Results.NotFound();
        }
        var shiftDtos = shifts.Select(e => new ShiftDto(e));
        return TypedResults.Ok(shiftDtos);
    }

    private static async Task<IResult> GetShiftsOnDateAsync(
        DateOnly dateUtc, 
        IShiftService shiftService,
        CancellationToken ct
    )
    {
        var shifts = await shiftService.GetShiftsOnDateAsync(dateUtc, ct);

        if (shifts == null || !shifts.Any())
        {
            return Results.NotFound();
        }
        var shiftDtos = shifts.Select(e => new ShiftDto(e));
        return TypedResults.Ok(shiftDtos);
    }

    private static async Task<IResult> AssignEmployeeToShiftAsync(
        Guid shiftId, Guid employeeId,
             IShiftService shiftService,
        CancellationToken ct
    )
    {
        var result = await shiftService.AssignEmployeeToShiftAsync(shiftId, employeeId, ct);

        if (result.ErrorCode == Error.NotFound)
        {
            return Results.NotFound(result.ErrorMessages);
        }
        else if (result.ErrorCode == Error.Conflict)
        {
            return Results.Conflict(result.ErrorMessages);
        }
        else if (result.ErrorCode != Error.None)
        {
            return Results.InternalServerError(result.ErrorMessages);
        }

        return TypedResults.Ok();
    }
}