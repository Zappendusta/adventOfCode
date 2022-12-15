using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day4
{
    private readonly ITestOutputHelper _output;
    private const int Day = 4;

    public Day4(ITestOutputHelper output)
    {
        _output = output;
    }

    private static bool AssignmentContainsDuplicate(string assignment)
    {
        var ranges = assignment.Split(",").Select(x =>
        {
            var ss = x.Split("-").Select(int.Parse).ToList();
            return (ss[0], ss[1]);
        }).ToList();
        var r1 = ranges[0];
        var r2 = ranges[1];

        return r1.Item1 >= r2.Item1 && r1.Item2 <= r2.Item2 ||
               r1.Item1 <= r2.Item1 && r1.Item2 >= r2.Item2;
    }
    
    private static bool AssignmentContainsOverlap(string assignment)
    {
        var ranges = assignment.Split(",").Select(x =>
        {
            var ss = x.Split("-").Select(int.Parse).ToList();
            return (ss[0], ss[1]);
        }).ToList();
        var r1 = ranges[0];
        var r2 = ranges[1];

        return r1.Item1 <= r2.Item2 && r1.Item2 >= r2.Item1;
    }

    private static string Solve1(IEnumerable<string> input)
    {
        return input.Select(AssignmentContainsDuplicate).Count(x => x == true).ToString();
    }

    private static string Solve2(IEnumerable<string> input)
    {
        return input.Select(AssignmentContainsOverlap).Count(x => x == true).ToString();
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "2";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "4";
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