using Demo.Domain.Exceptions;
using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using Demo.Domain.Validators;

namespace Demo.Domain.Services;

public class EmployeeService(IEmployeeRepository employeeStorageService) : IEmployeeService
{
    public async Task<Employee> CreateEmployeeAsync(string name, string lastname, CancellationToken cancellationToken = default) 
    { 
        var newEmployee = new Employee(name, lastname);
        var errorMessages = EmployeeValidator.Validate(newEmployee);

        if (errorMessages.Any())
        {
            throw new ValidationException(errorMessages);
        }

        var existingUser = await employeeStorageService.GetEmployeeAsync(name, lastname, cancellationToken);
        if (existingUser is not null)
        {
            throw new ConflictException(["Employee already exists"]);
        }

        return await employeeStorageService.CreateEmployeeAsync(newEmployee, cancellationToken);
    }

    public async Task<Employee?> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default) 
    { 
        return await employeeStorageService.GetEmployeeAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageSize = 0, int pageNum = 0, CancellationToken cancellationToken = default) 
    { 
        return await employeeStorageService.GetEmployeesAsync(pageSize, pageNum, cancellationToken);
    }
}