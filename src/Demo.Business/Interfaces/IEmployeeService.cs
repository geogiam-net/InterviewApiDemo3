using Demo.Business.Models;

namespace Demo.Business.Interfaces;

public interface IEmployeeService
{
    public Task<Employee> CreateEmployeeAsync(string name, string lastname, CancellationToken cancellationToken = default);

    public Task<Employee?> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Employee>> GetEmployeesAsync(int pageSize = 0, int pageNum = 0, CancellationToken cancellationToken = default);

}