public record Employee(string Name, string Type, decimal Rate, decimal Hours, decimal Sales, string State);

public class EmployeeParser
{
    public(List<Employee> Valid, List<string> Error) Parse(string rawinput)
    {
        var Valid = new List<Employee>();
        var Error = new List<string>();

        //* break the string based on delimiter
        foreach(var r in rawinput.Split('\n'))
        {
            var data = r.Split(",");

            if (data.Length != 6)
            {
                Error.Add($"String has too many things ref: {r}");
                continue;
            }

            if ((!decimal.TryParse(data[2], out var Rate)) || (!decimal.TryParse(data[3], out var Hours)) || (!decimal.TryParse(data[4], out var Sales)))
            {
                Error.Add($"Non-Numerical number detected on {data[0]}");
                continue;
            }

            Valid.Add(new Employee(data[0], data[1], Rate,Hours,Sales,data[5]));
        }
        return (Valid, Error);
    }
}



//string raw = "Ana,H,20,45,0,TX\nBen,S,130000,40,0,CA\nCara,C,52000,40,10000,NY\nDev,H,18.5,38,0,CA\nEli,H,abc,40,0,TX\nFay,S,65000,40,0,WA";