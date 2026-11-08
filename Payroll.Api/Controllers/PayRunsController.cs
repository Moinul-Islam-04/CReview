using Microsoft.AspNetCore.Mvc;
using Payroll.Api.Dtos;
using Payroll.Api.Services;

namespace Payroll.Api.Controllers;

[ApiController]
[Route("api/payruns")]
public class PayRunsController(IPayrollService payroll) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PayRunResult>> Run(RunPayrollRequest req, CancellationToken ct)
    {
        try
        {
            return Ok(await payroll.RunPayrollAsync(req.WeekStart, ct));
        }
        catch (PayRunAlreadyExistsException ex)
        {
            return Conflict(new ProblemDetails { Title = "Pay run already exists", Detail=ex.Message, Status = 409});
        }
    }
}