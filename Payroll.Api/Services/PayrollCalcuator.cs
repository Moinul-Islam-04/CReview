namespace Payroll.Api.Services;

public interface IPayrollCalculator
{
    decimal CalculateGross(decimal hours, decimal rate);
}

public class PayrollCalculator : IPayrollCalculator
{
    public decimal CalculateGross(decimal hours, decimal rate)
    {
        var regular = Math.Min(hours, 40m) * rate;
        var overtime = Math.Max(hours - 40m, 0m) * rate * 1.5m;
        return Math.Round(regular + overtime, 2, MidpointRounding.AwayFromZero);
    }
}
