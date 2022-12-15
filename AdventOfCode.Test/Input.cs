using System.Reflection;

namespace AdventOfCode.Test;

public static class Input
{
    public static string[] Get(int day, bool test = true)
    {
        var fileName = $"day{day}";
        var path = Path.Combine(@"C:\Users\Paulus\RiderProjects\AdventOfCode\AdventOfCode.Test\Inputs\", fileName);

        string[] ReadFile(string end)
        {
            return File.ReadAllText($"{path}{end}.txt").Split("\r\n");
        }

        return ReadFile(test ? "_test" : "");
    }
}