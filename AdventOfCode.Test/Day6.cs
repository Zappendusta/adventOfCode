using System.Collections;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day6
{
    private readonly ITestOutputHelper _output;
    private const int Day = 6;

    public Day6(ITestOutputHelper output)
    {
        _output = output;
    }

    private static bool AllUnique(IEnumerable<char> input)
    {
        return input.Distinct().Count() == input.Count();
    }

    private static string DetectStartOfNUnuique(IEnumerable<string> input, int n)
    {
        var enumerable = input.First().ToList();
        for (var i = 0; i < enumerable.Count(); i++)
        {
            if (AllUnique(enumerable.Skip(i).Take(n)))
            {
                return (i + n).ToString();
            }
        }

        throw new ArgumentOutOfRangeException();
    }

    private static string Solve1(IEnumerable<string> input)
    {
        return DetectStartOfNUnuique(input, 4);
    }


    private static string Solve2(IEnumerable<string> input)
    {
        return DetectStartOfNUnuique(input, 14);
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "7";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "19";
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