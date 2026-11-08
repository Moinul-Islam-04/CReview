using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payroll.Api.Data;

namespace Payroll.Tests;

public class PayrollApiFactory : WebApplicationFactory<Program>
{
    public PayrollApiFactory()
    {
        Environment.SetEnvironmentVariable("ConnectionStrings__Payroll",
            "Server=localhost,1433;Database=Payroll_Test;User Id=sa;Password=Payroll_Pass2026;TrustServerCertificate=True");

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PayrollDbContext>();
        db.Database.EnsureDeleted();
        db.Database.Migrate();
    }
}
