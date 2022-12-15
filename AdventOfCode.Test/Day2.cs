using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day2
{
    private readonly ITestOutputHelper _output;
    private const int Day = 2;

    public Day2(ITestOutputHelper output)
    {
        _output = output;
    }


    private static IEnumerable<(char, char)> ParseInput(string input)
    {
        return input.Replace("\r\n", "\n").Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(x =>
        {
            var split = x.Trim().Split(" ");
            return (split[0].First(), split[1].First());
        });
    }

    private static readonly Dictionary<string, int> Outcomes = new()
    {
        { "A X", 4 },
        { "A Y", 8 },
        { "A Z", 3 },
        { "B X", 1 },
        { "B Y", 5 },
        { "B Z", 9 },
        { "C X", 7 },
        { "C Y", 2 },
        { "C Z", 6 }
    };

    private static readonly Dictionary<string, string> Moves = new()
    {
        { "A X", "A Z" },
        { "A Y", "A X" },
        { "A Z", "A Y" },
        { "B X", "B X" },
        { "B Y", "B Y" },
        { "B Z", "B Z" },
        { "C X", "C Y" },
        { "C Y", "C Z" },
        { "C Z", "C X" }
    };

    private static string Solve1(IEnumerable<string> input)
    {
        var ans = input
            .Select(x => Outcomes[x])
            .ToList();
        return ans.Sum().ToString();
    }

    private static string Solve2(IEnumerable<string> input)
    {
        var ans = input
            .Select(x => Moves[x])
            .Select(x => Outcomes[x])
            .ToList();
        return ans.Sum().ToString();
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "15";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "12";
            result.Should().Be(expected);
        }
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