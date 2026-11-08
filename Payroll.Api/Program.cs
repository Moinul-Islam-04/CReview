using Microsoft.EntityFrameworkCore;
using Payroll.Api.Data;
using Payroll.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<PayrollDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("Payroll")));
builder.Services.AddSingleton<IPayrollCalculator, PayrollCalculator>(); //Metrics
builder.Services.AddScoped<IPayrollService, PayrollService>(); // DB stuff
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }