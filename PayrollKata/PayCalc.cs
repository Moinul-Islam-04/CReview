public interface IPaycalc
{
     decimal Calculate(Employee e);
}

public class HourlyPay : IPaycalc
{
    private const decimal RegularHours = 40m;
    private const decimal OvertimeMultiplier = 1.5m;
    public decimal Calculate(Employee e)
    {
       if (e.Hours <= RegularHours)
            return e.Rate * e.Hours;

        var OvertimeHours = e.Hours - RegularHours;
        return RegularHours * e.Rate + OvertimeHours * e.Rate * OvertimeMultiplier;
    }   

}

public class SalariedPay : IPaycalc
{
    private const decimal WeeksPerYear = 52;
    public decimal Calculate(Employee e) => e.Rate/WeeksPerYear;
}

public class ComissionedPay : IPaycalc
{
    private const decimal SaleRate = 0.05m;
    private const decimal WeeksPerYear = 52;
    public decimal Calculate(Employee e)
    {
        return e.Rate / WeeksPerYear + e.Sales * SaleRate;
     
    }

}

public class ContractorPay : IPaycalc
{
    private const decimal HoursPerDay = 8m;
    public decimal Calculate(Employee e)
    {
        return Math.Ceiling(e.Hours / HoursPerDay) * e.Rate;
    } 
}