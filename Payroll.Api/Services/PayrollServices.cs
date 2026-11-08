using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Dtos;
using Payroll.Api.Models;

namespace Payroll.Api.Services;

public class DuplicateTimeSheetException(int employeeId, DateOnly week)
    : Exception($"Employee {employeeId} already has a timesheets for {week}");

public class PayRunAlreadyExistsException(DateOnly week)
    : Exception($"Payroll has already been run for the week of {week}.");

public interface IPayrollService
{
    Task<EmployeeDto?> GetEmployeeAsync(int Id, CancellationToken ct);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeRequest req, CancellationToken ct);
    Task<TimesheetDto> AddTimesheetAsync(int employeeId, AddTimeSheetRequest req, CancellationToken ct);

    Task<PayRunResult> RunPayrollAsync(DateOnly weekStart, CancellationToken ct);

}

public class PayrollService(PayrollDbContext db, IPayrollCalculator calc) : IPayrollService
{
    public async Task<EmployeeDto?> GetEmployeeAsync(int id, CancellationToken ct) =>
        await db.Employees
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDto(e.Id, e.Name, e.HourlyRate))
            .FirstOrDefaultAsync(ct);
    
    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeRequest req, CancellationToken ct)
    {
        var emp = new Employee {Name = req.Name, HourlyRate = req.HourlyRate};
        db.Employees.Add(emp);
        await db.SaveChangesAsync(ct);
        return new EmployeeDto(emp.Id, emp.Name, emp.HourlyRate);
    }

    public async Task<TimesheetDto> AddTimesheetAsync(int employeeId, AddTimeSheetRequest req, CancellationToken ct)
    {
        if (!await db.Employees.AnyAsync(e => e.Id == employeeId, ct)) return null;

        var ts = new Timesheet { EmployeeId = employeeId, WeekStart = req.WeekStart, Hours = req.Hours };
        db.Timesheets.Add(ts);
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)   // the unique index rejected a second timesheet for that week
        {
            throw new DuplicateTimeSheetException(employeeId, req.WeekStart);
        }
        return new TimesheetDto(ts.Id, ts.EmployeeId, ts.WeekStart, ts.Hours);
    }

    public async Task<PayRunResult> RunPayrollAsync(DateOnly weekStart, CancellationToken ct)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var row = await db.Timesheets
            .Where(t => t.WeekStart == weekStart)
            .Join(db.Employees, t => t.EmployeeId, e => e.Id, (t,e) => new { t.EmployeeId, t.Hours, e.HourlyRate})
            .ToListAsync(ct);
        
        var stub = row.Select(r => new Paystub
        {
            EmployeeId = r.EmployeeId,
            WeekStart = weekStart,
            Gross = calc.CalculateGross(r.Hours, r.HourlyRate)
        }).ToList();

        db.Paystubs.AddRange(stub);

        try
        {
            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch (DbUpdateException)
        {
            await tx.RollbackAsync(ct);
            throw new PayRunAlreadyExistsException(weekStart);
        }

        var dtos = stub.Select(s => new PaystubDto(s.EmployeeId, s.WeekStart, s.Gross)).ToList();
        return new PayRunResult(weekStart, dtos.Count, dtos.Sum(d => d.Gross), dtos);
    }
    
}