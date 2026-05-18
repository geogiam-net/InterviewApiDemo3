using Demo.Business.Models;

namespace Demo.Business.Interfaces;

public interface IEmployeeRepository
{
    public Task<Employee> CreateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);

    public Task<Employee?> GetEmployeeAsync(string name, string lastname, CancellationToken cancellationToken = default);

    public Task<Employee?> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Employee>> GetEmployeesAsync(int pageSize = 0, int pageNum = 0, CancellationToken cancellationToken = default);

}