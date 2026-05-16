using Microsoft.EntityFrameworkCore;
using Demo.Domain.Models;

namespace Demo.Infrastructure.SqlStorage.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public const string DbSchema = "DemoApp";

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Shift> Shifts => Set<Shift>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DbSchema);

        modelBuilder.Entity<Employee>().HasKey(x => x.Id);
        modelBuilder.Entity<Employee>().Property(x => x.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Employee>().Property(x => x.Lastname).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Employee>().HasIndex(x => new { x.Name, x.Lastname });

        modelBuilder.Entity<Shift>().HasKey(x => x.Id);
        modelBuilder.Entity<Shift>().Property(x => x.Name).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<Shift>().Property(x => x.StartTime).IsRequired();
        modelBuilder.Entity<Shift>().Property(x => x.EndTime).IsRequired();

        // Cascade delete of employees in Shifts are not needed because in this demo we will not delete employees, also In-Memory dabase does not support it

        base.OnModelCreating(modelBuilder);
    }
}
