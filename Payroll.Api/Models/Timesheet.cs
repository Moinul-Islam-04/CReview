namespace Payroll.Api.Models;

public class Timesheet
{
    public int Id {get; set;}
    public int EmployeeId {get; set;} 
    // FK to Employees

    public DateOnly WeekStart {get; set;}

    public decimal Hours { get; set; }
}