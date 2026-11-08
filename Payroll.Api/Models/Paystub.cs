namespace Payroll.Api.Models;

public class Paystub
{
    public int Id {get; set;}
    public int EmployeeId {get; set;}

    public DateOnly WeekStart {get; set;}

    public decimal Gross {get; set;}
}