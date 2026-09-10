using Demo.Application.Dtos;
using Demo.Domain.Models;

namespace Demo.Application.Interfaces;

public interface IEmployeeService
{
    public Task<ResultDto<Employee?>> CreateEmployeeAsync(
        string name, 
        string lastname, 
        CancellationToken ct);

    public Task<ResultDto<Employee?>> GetEmployeeAsync(Guid id, CancellationToken ct);

    public Task<IEnumerable<Employee>> GetEmployeesAsync(
        int pageSize = 0, 
        int pageNum = 0, 
        CancellationToken ct = default);

}