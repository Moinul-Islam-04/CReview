namespace Payroll.Api.Models;

public class Employee
{
    public int Id {get; set;} // Primary Key
    public required string Name {get; set;}

    public decimal HourlyRate {get; set;}

    public List<Timesheet> Timesheets {get; set;} = [];
    // one employee - > many timesheets
}