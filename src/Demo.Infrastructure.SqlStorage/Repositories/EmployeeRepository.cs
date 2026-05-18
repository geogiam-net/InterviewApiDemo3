using Microsoft.EntityFrameworkCore;
using Demo.Business.Interfaces;
using Demo.Business.Models;
using Demo.Infrastructure.SqlStorage.Data;

namespace Demo.Infrastructure.SqlStorage.Services;

public class EmployeeRepository(ApplicationDbContext dbContext) : IEmployeeRepository
{
    public async Task<Employee> CreateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task<Employee?> GetEmployeeAsync(string name, string lastname, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name && x.Lastname == lastname, cancellationToken);
    }

    public async Task<Employee?> GetEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageSize = 0, int pageNum = 0, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Employees.AsNoTracking();

        if (pageNum > 0)
        {
            query = query.Skip(pageNum * (pageSize < 0 ? 0 : pageSize));
        }

        if (pageSize > 0)
        {
            query = query.Take(pageSize);
        }

        query = query.OrderByDescending(x => x.Lastname).ThenByDescending(x => x.Name);

        return await query.ToListAsync(cancellationToken);
    }
}