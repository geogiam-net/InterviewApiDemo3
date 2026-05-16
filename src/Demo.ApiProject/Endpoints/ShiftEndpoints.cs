using Demo.Api.Dtos;
using Demo.Domain.Interfaces;
using Demo.Domain.Models;

namespace Demo.Api.Endpoints;

internal static class ShiftEndpoints
{
    internal static void MapShiftEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/shifts",
            async Task<IResult> (string name, Role role, DateTime startTimeUtc, DateTime endTimeUtc, 
            IShiftService shiftService, CancellationToken cancellationToken) =>
            {
                var newShift = await shiftService.CreateShiftAsync(name, role, startTimeUtc, endTimeUtc, cancellationToken);

                // return 201 with link to created resource
                return TypedResults.Created(
                          uri: $"/api/shifts/{newShift.Id}",
                          value: new ShiftDto(newShift));
            });

        // Not in requirements but added because we reference the url when we create a new shift
        builder.MapGet("/api/shifts/{id}",
            async Task<IResult> (Guid id,
            IShiftService shiftService, CancellationToken cancellationToken) =>
            {
                var shift = await shiftService.GetShiftAsync(id, cancellationToken);
                if (shift is null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(new ShiftDto(shift));
            });

        builder.MapGet("/api/shifts/employee/{employeeId}",
            async Task<IResult> (Guid employeeId,
            IShiftService shiftService, CancellationToken cancellationToken) =>
            {
                var shifts = await shiftService.GetShiftsFromEmployeeAsync(employeeId, cancellationToken);
                var shiftDtos = shifts.Select(e => new ShiftDto(e));
                return TypedResults.Ok(shiftDtos);
            });

        builder.MapGet("/api/shifts/date/{date}",
            async Task<IResult> (DateOnly dateUtc, IShiftService shiftService, CancellationToken cancellationToken) =>
            {
                var shifts = await shiftService.GetShiftsOnDateAsync(dateUtc, cancellationToken);
                var shiftDtos = shifts.Select(e => new ShiftDto(e));
                return TypedResults.Ok(shiftDtos);
            });

        builder.MapPut("/api/shifts/{shiftId}/employee/{employeeId}",
             async Task<IResult> (Guid shiftId, Guid employeeId,
             IShiftService shiftService, CancellationToken cancellationToken) =>
             {
                 await shiftService.AssignEmployeeToShiftAsync(shiftId, employeeId, cancellationToken);
                 return TypedResults.Ok();
             });
    }
}