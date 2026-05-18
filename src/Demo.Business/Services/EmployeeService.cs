using Demo.Business.Exceptions;
using Demo.Business.Interfaces;
using Demo.Business.Models;
using Demo.Business.Validators;

namespace Demo.Business.Services;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task<Employee> CreateEmployeeAsync(string name, string lastname, CancellationToken cancellationToken = default) 
    { 
        var newEmployee = new Employee(name, lastname);
        var errorMessages = EmployeeValidator.Validate(newEmployee);

        if (errorMessages.Any())
        {
            throw new ValidationException(errorMessages);
        }

        var existingUser = await employeeRepository.GetEmployeeAsync(name, lastname, cancellationToken);
        if (existingUser is not null)
        {
            throw new ConflictException(["Employee already exists"]);
        }

        return await employeeRepository.CreateEmployeeAsync(newEmployee, cancellationToken);
    }

    public async Task<Employee?> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default) 
    { 
        return await employeeRepository.GetEmployeeAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageSize = 0, int pageNum = 0, CancellationToken cancellationToken = default) 
    { 
        return await employeeRepository.GetEmployeesAsync(pageSize, pageNum, cancellationToken);
    }
}