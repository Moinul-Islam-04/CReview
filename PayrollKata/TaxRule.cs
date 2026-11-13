using System.Runtime.CompilerServices;

public interface ITaxRule
{
    decimal Calculate(decimal gross);
}

public class FlatRateTax : ITaxRule
{
    private readonly decimal _rate;

    public FlatRateTax(decimal rate)
    {
        if (rate < 0 || rate > 1)
            throw new ArgumentOutOfRangeException(nameof(rate), "Rate must be between 0 and 1");
        _rate = rate;
    }

    public decimal Calculate(decimal gross) => gross * _rate;

}

public class NoStateIncomeTax : ITaxRule
{
    public decimal Calculate(decimal gross) => 0m;
}