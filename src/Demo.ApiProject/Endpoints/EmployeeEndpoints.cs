
using Demo.Api.Dtos;
using Demo.Domain.Interfaces;

namespace Demo.Api.Endpoints;

internal static class EmployeeEndpoints
{
    internal static void MapEmployeeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/employees",
            async Task<IResult> (string name, string lastname, 
            IEmployeeService employeeService, CancellationToken cancellationToken) =>
            {
                var newEmployee = await employeeService.CreateEmployeeAsync(name, lastname, cancellationToken);

                // return 201 with link to created resource
                return TypedResults.Created(
                          uri: $"/api/employees/{newEmployee.Id}",
                          value: new EmployeeDto(newEmployee));
            });

        builder.MapGet("/api/employees/{id}",
            async Task<IResult> (Guid id, 
            IEmployeeService employeeService, CancellationToken cancellationToken) =>
            {
                var employee = await employeeService.GetEmployeeAsync(id, cancellationToken);
                if (employee is null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(new EmployeeDto(employee));
            });

        builder.MapGet("/api/employees/",
            async Task<IResult> (int? pageSize, int? pageNum, 
            IEmployeeService employeeService, CancellationToken cancellationToken) =>
            {
                var employees = await employeeService.GetEmployeesAsync(pageSize ?? 0, pageNum ?? 0, cancellationToken);
                var employeeDtos = employees.Select(e => new EmployeeDto(e));
                return TypedResults.Ok(employeeDtos);
            });
    }
}