using System.Diagnostics;

public class ConsoleReport
{
    private const decimal HighEarnerThreshold = 2000m;

    public void Print(List<PayResult> results, List<string> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"ERROR: {error}");
        }
        foreach (var result in results)
        {
               Console.WriteLine($"{result.Name} ({result.Type}) gross: {result.Gross:N2} tax: {result.Tax:N2} net: {result.Net:N2}");
            if (result.Gross > HighEarnerThreshold)
            {
                Console.WriteLine("** HIGH EARNER - needs manager approval **");
            }
        }

        decimal total = results.Sum(r => r.Gross);
           Console.WriteLine($"TOTAL PAYROLL: {total:C}");
    }
}