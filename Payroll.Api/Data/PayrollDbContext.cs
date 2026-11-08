using Microsoft.EntityFrameworkCore;
using Payroll.Api.Models;

namespace Payroll.Api.Data;

public class PayrollDbContext(DbContextOptions<PayrollDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Timesheet> Timesheets => Set<Timesheet>();
    public DbSet<Paystub> Paystubs => Set<Paystub>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Employee>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(100);
            e.Property(x => x.HourlyRate).HasPrecision(18,2);
        });

        b.Entity<Timesheet>(t =>
        {
            t.Property(x => x.Hours).HasPrecision(5,2);
            t.HasIndex(x => new { x.EmployeeId, x.WeekStart}).IsUnique();
        });

        b.Entity<Paystub>(p =>
        {
            p.Property(x => x.Gross).HasPrecision(18,2);
            p.HasIndex(x => new {x.EmployeeId, x.WeekStart}).IsUnique();
            p.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId);
        });

        
        
        
        
        
    }

}