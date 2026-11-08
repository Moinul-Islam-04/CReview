using Payroll.Api.Services;

namespace Payroll.Tests;

public class PayrollCalculatorTests
{
    private readonly PayrollCalculator _calc = new();

    [Fact] //What is a FACT
    public void Forty_hours_has_no_overtime()
        => Assert.Equal(800m, _calc.CalculateGross(40m, 20m));
    
    [Theory] // What is a theory
    [InlineData(45, 20, 950)] // Inputs + outcomes
    [InlineData(0,20,0)]
    [InlineData(40.5, 20, 830)]

    public void overtime_is_time_and_a_half(decimal hours, decimal rate, decimal expected)
        => Assert.Equal(expected, _calc.CalculateGross(hours,rate));


}