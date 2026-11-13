public record PayResult(string Name, string Type, decimal Gross, decimal Tax, decimal Net);

public class PayrollRunner
{
    private readonly Dictionary<string, IPaycalc> _calcultors;
    private readonly Dictionary<string, ITaxRule> _taxRules;

    public PayrollRunner( Dictionary<string, IPaycalc> calcs, Dictionary<string, ITaxRule> tax)
    {
        _calcultors = calcs;
        _taxRules = tax;
    }

    public (List<PayResult> results, List<string> Errors) Run(List<Employee> employees)
    {
        var result = new List<PayResult>();
        var errors = new List<string>();

        foreach (var e in employees)
        {
            if(!_calcultors.TryGetValue(e.Type, out var calculator))
            {
                errors.Add($"unkown Pay type {e.Type} for {e.Name}");
                continue;
            }
            decimal pay = calculator.Calculate(e);

            if(!_taxRules.TryGetValue(e.State, out var taxRule))
            {
                errors.Add($"unkown state {e.State} for {e.Name}");
                continue;
            }

            decimal tax = taxRule.Calculate(pay);

            var net = pay - tax;

            result.Add(new PayResult(e.Name, e.Type, pay, tax, net));
        }

        return (result, errors);
    }
}