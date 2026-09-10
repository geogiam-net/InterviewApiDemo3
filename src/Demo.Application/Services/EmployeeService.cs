using Demo.Application.Dtos;
using Demo.Application.Interfaces;
using Demo.Domain.Enums;
using Demo.Domain.Models;
using Demo.Domain.Validators;

namespace Demo.Application.Services;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task<ResultDto<Employee?>> CreateEmployeeAsync(
        string name, 
        string lastname, 
        CancellationToken ct) 
    { 
        var newEmployee = new Employee(name, lastname);
        var errorMessages = EmployeeValidator.Validate(newEmployee);

        if (errorMessages.Any())
        {
            return new ResultDto<Employee?>(null, Error.ValidationError, errorMessages);
        }

        var existingUser = await employeeRepository.GetEmployeeAsync(name, lastname, ct);
        if (existingUser is not null)
        {
            return new ResultDto<Employee?>(
                null, 
                Error.Conflict, 
                new List<string> { "Employee already exists" });
        }

        return new ResultDto<Employee?>(await employeeRepository.CreateEmployeeAsync(newEmployee, ct));
    }

    public async Task<ResultDto<Employee?>> GetEmployeeAsync(Guid id, CancellationToken ct) 
    {
        var existingUser = await employeeRepository.GetEmployeeAsync(id, ct);
        if (existingUser is null)
        {
            return new ResultDto<Employee?>(
                null,
                Error.NotFound,
                new List<string> { "Employee not found" });
        }

        return new ResultDto<Employee?>(existingUser);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(
        int pageSize = 0, 
        int pageNum = 0, 
        CancellationToken ct = default) 
    { 
        return await employeeRepository.GetEmployeesAsync(pageSize, pageNum, ct);
    }
}