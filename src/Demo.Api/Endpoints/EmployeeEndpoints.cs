
using Demo.Api.Dtos;
using Demo.Application.Interfaces;
using Demo.Domain.Enums;

namespace Demo.Api.Endpoints;

internal static class EmployeeEndpoints
{
    internal static void MapEmployeeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Routes.Employees, CreateEmployeeAsync);
        builder.MapGet(Routes.EmployeeById, GetEmployeeByIdAsync);
        builder.MapGet(Routes.Employees, GetEmployeesAsync);
    }

    private static async Task<IResult> CreateEmployeeAsync(
            NewEmployeeDto employee,
            IEmployeeService employeeService, 
            CancellationToken ct
        )
    {
        var result = await employeeService.CreateEmployeeAsync(employee.Name, employee.Lastname, ct);

        if (result.ErrorCode == Error.ValidationError)
        {
            return Results.BadRequest(result.ErrorMessages);
        }
        else if (result.ErrorCode == Error.Conflict)
        {
            return Results.Conflict(result.ErrorMessages);
        }
        else  if (result.ErrorCode != Error.None)
        {
            return Results.InternalServerError(result.ErrorMessages);
        }

        // return 201 with link to created resource
        return TypedResults.Created(
                  uri: $"{Routes.Employees}/{result.Result!.Id}",
                  value: new EmployeeDto(result.Result));
    }

    private static async Task<IResult> GetEmployeeByIdAsync(
            Guid id,
            IEmployeeService employeeService,
            CancellationToken ct
        )
    {
        var result = await employeeService.GetEmployeeAsync(id, ct);
        if (result.ErrorCode == Error.NotFound)
        {
            return Results.NotFound(result.ErrorMessages);
        }
        else if (result.ErrorCode != Error.None)
        {
            return Results.InternalServerError(result.ErrorMessages);
        }

        return TypedResults.Ok(new EmployeeDto(result.Result!));
    }

    private static async Task<IResult> GetEmployeesAsync(
        int? pageSize,
        int? pageNum,
        IEmployeeService employeeService,
        CancellationToken ct
    )
    {
        var employees = await employeeService.GetEmployeesAsync(pageSize ?? 0, pageNum ?? 0, ct);
        if(employees == null || !employees.Any())
        {
            return Results.NotFound();
        }
        var employeeDtos = employees.Select(e => new EmployeeDto(e));
        return TypedResults.Ok(employeeDtos);
    }
}