using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day1
{
    private readonly ITestOutputHelper _output;
    private const int Day = 1;

    public Day1(ITestOutputHelper output)
    {
        _output = output;
    }
    
    private static IEnumerable<int> CaloriesPerElf(IEnumerable<string> input)
    {
        var current = 0;
        foreach (var s in input)
        {
            if (int.TryParse(s, out var x))
            {
                current += x;
            }
            else
            {
                yield return current;
                current = 0;
            }
        }
    }

    private static string Solve1(IEnumerable<string> input)
    {
        var all = CaloriesPerElf(input).OrderByDescending(x => x).ToList();
        return all.First().ToString();
    }

    private static string Solve2(IEnumerable<string> input)
    {
        return CaloriesPerElf(input).Take(3).Sum().ToString();
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        var result = Solve1(input);
        const string expected = "24000";
        result.Should().Be(expected);
    }

    [Fact]
    public void TestSolve1()
    {
        var input = Input.Get(Day, false);
        var result = Solve1(input);
        _output.WriteLine(result);
    }

    [Fact]
    public void TestSolve2()
    {
        var input = Input.Get(Day, false);
        var result = Solve2(input);
        _output.WriteLine(result);
    }
}