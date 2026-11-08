namespace Payroll.Api.Dtos;

public record RunPayrollRequest(DateOnly WeekStart);
public record PaystubDto(int EmployeeId, DateOnly WeekStart, decimal Gross);
public record PayRunResult(DateOnly WeekStart, int PaystubCount, decimal TotalGross, IReadOnlyList<PaystubDto> Paystubs);