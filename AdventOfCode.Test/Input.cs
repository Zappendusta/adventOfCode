using System.Reflection;

namespace AdventOfCode.Test;

public static class Input
{
    public static string[] Get(int day, bool test = true, bool testTwo = false)
    {
        var fileName = $"day{day}";
        var path = Path.Combine("/Users/paulusdettmer/dev/adventOfCode/AdventOfCode.Test/Inputs/", fileName);

        string[] ReadFile(string end)
        {
            return File.ReadAllText($"{path}{end}.txt").Split("\r\n").SelectMany(x=>x.Split("\n")).ToArray();
        }

        return testTwo ? ReadFile("_test2" ) : ReadFile(test ? "_test" : "");
    }
}