
using Demo.Api.Dtos;
using Demo.Application.Interfaces;

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

        return ResultDtoResultMapper.ToHttpResult(
            result,
            employee => TypedResults.Created(
                uri: $"{Routes.Employees}/{employee!.Id}",
                value: new EmployeeDto(employee)));
    }

    private static async Task<IResult> GetEmployeeByIdAsync(
            Guid id,
            IEmployeeService employeeService,
            CancellationToken ct
        )
    {
        var result = await employeeService.GetEmployeeAsync(id, ct);
        return ResultDtoResultMapper.ToHttpResult(
            result,
            employee => TypedResults.Ok(new EmployeeDto(employee!)));
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
