using Microsoft.AspNetCore.Mvc;
using Payroll.Api.Dtos;
using Payroll.Api.Services;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController(IPayrollService payroll) : ControllerBase
{
    // GET /api/employees/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> Get(int id, CancellationToken ct)
    {
        var emp = await payroll.GetEmployeeAsync(id, ct);
        return emp is null ? NotFound() : Ok(emp);
    }

    // POST /api/employees
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeRequest req, CancellationToken ct)
    {
        var created = await payroll.CreateEmployeeAsync(req, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id}, created);
    }

    [HttpPost("{id:int}/timesheets")]
    public async Task<ActionResult<TimesheetDto>> AddTimesheet(int id, AddTimeSheetRequest req, CancellationToken ct)
    {
        try
        {
            var ts = await payroll.AddTimesheetAsync(id, req, ct);
            return ts is null ? NotFound() : StatusCode(StatusCodes.Status201Created, ts);
        }
        catch (DuplicateTimeSheetException ex)
        {
            return Conflict(new ProblemDetails { Title = "Duplicate timesheet", Detail = ex.Message, Status = 409});
        }
    }

}