using Demo.Application.Interfaces;
using Demo.Application.Services;
using Demo.Infrastructure.SqlRepository.Data;
using Demo.Infrastructure.SqlRepository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Startup;

public static class DependencyInjection
{
    public const string DatabaseName = "DemoDb";

    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(DatabaseName));
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IShiftService, ShiftService>();

        return services;
    }

    // public static IServiceCollection AddWorkers .....
}
