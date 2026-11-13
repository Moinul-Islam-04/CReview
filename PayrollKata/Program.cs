using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


class Program
{
   

    static void Main(string[] args)
    {
        // name,type,rate,hours,sales,state
        string raw = "Ana,H,20,45,0,TX\nBen,S,130000,40,0,CA\nCara,C,52000,40,10000,NY\nDev,H,18.5,38,0,CA\nEli,H,abc,40,0,TX\nFay,S,65000,40,0,WA\nGus,X,20,40,0,TX\nGus2,K,300,20,0,TX";

        var calculators = new Dictionary<string, IPaycalc>
        {
            ["H"] = new HourlyPay(),
            ["S"] = new SalariedPay(),
            ["C"] = new ComissionedPay(),
            ["K"] = new ContractorPay(),
        };

        var taxes = new Dictionary<string, ITaxRule>
        {
            ["TX"] = new NoStateIncomeTax(),
            ["CA"] = new FlatRateTax(0.06m),
            ["NY"] = new FlatRateTax(0.05m),
        };


        var(employee, errors) = new EmployeeParser().Parse(raw);
        var runner = new PayrollRunner(calculators, taxes);
        var (result, runerrors) = runner.Run(employee);
        new ConsoleReport().Print(result, [.. errors, .. runerrors]);
       
}
}


/* Current Output:
Ana (H) gross: 950 tax: 0 net: 950
Ben (S) gross: 2500 tax: 150 net: 2350
  ** HIGH EARNER - needs manager approval **
Cara (C) gross: 1500 tax: 75 net: 1425
Dev (H) gross: 703 tax: 42.18 net: 660.82
Fay (S) gross: 1250 tax: 0 net: 1250
TOTAL PAYROLL: 6903
*/
/*
TODO 
- 
*/

