using Demo.Api.Dtos;
using Demo.Application.Interfaces;

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

        return ResultDtoResultMapper.ToHttpResult(
            result,
            shift => TypedResults.Created(
                uri: $"{Routes.Shifts}/{shift!.Id}",
                value: new ShiftDto(shift)));
    }

    private static async Task<IResult> ShiftByIdAsync(
        Guid id,
        IShiftService shiftService,
        CancellationToken ct
    )
    {
        var result = await shiftService.GetShiftAsync(id, ct);

        return ResultDtoResultMapper.ToHttpResult(
            result,
            shift => TypedResults.Ok(new ShiftDto(shift!)));
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

        return ResultDtoResultMapper.ToHttpResult(
            result,
            _ => TypedResults.Ok());
    }
}
