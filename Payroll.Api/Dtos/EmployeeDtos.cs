using System.ComponentModel.DataAnnotations;

namespace Payroll.Api.Dtos;

public record CreateEmployeeRequest(
    [Required, StringLength(100)] string Name,
    [Range(7.25,500)] decimal HourlyRate);

public record EmployeeDto(int Id, string Name, decimal HourlyRate);
public record AddTimeSheetRequest(DateOnly WeekStart, [Range(0,168)] decimal Hours);
public record TimesheetDto(int Id, int EmployeeId, DateOnly WeekStart, decimal Hours);