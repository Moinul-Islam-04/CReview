using System.Net;
using System.Net.Http.Json;
using Payroll.Api.Dtos;

namespace Payroll.Tests;

public class PayrollAPiTest(PayrollApiFactory factory) : IClassFixture<PayrollApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Unkown_employee_return_404()
    {
        var res = await _client.GetAsync("/api/employees/999999");
        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Invalid_employe_returns_400()
    {
        var res = await _client.PostAsJsonAsync("/api/employees", new { name = "", hourlyRate = 2});
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Pay_run_pays_once_and_rejects_a_second_run()
    {
        var craeted = await _client.PostAsJsonAsync("/api/employees", new { name = "Ana", hourlyRate = 20});
        var emp = await craeted.Content.ReadFromJsonAsync<EmployeeDto>();
        await _client.PostAsJsonAsync($"/api/employees/{emp!.Id}/timesheets",
            new { weekStart = "2026-11-02", hours = 45});

        var first = await _client.PostAsJsonAsync("/api/payruns", new { weekStart = "2026-11-02"});
        var second = await _client.PostAsJsonAsync("/api/payruns", new { weekStart = "2026-11-02"});

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        var result = await first.Content.ReadFromJsonAsync<PayRunResult>();
        Assert.Equal(950m, result!.TotalGross);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);

    }
}