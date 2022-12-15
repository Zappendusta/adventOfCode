using System.Collections;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day0
{
    private readonly ITestOutputHelper _output;
    private const int Day = 0;

    public Day0(ITestOutputHelper output)
    {
        _output = output;
    }


    private static string Solve1(IEnumerable<string> input)
    {
        return "";
    }


    private static string Solve2(IEnumerable<string> input)
    {
        return "";
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "";
            result.Should().Be(expected);
        }
        // {
        //     var result = Solve2(input);
        //     const string expected = "19";
        //     result.Should().Be(expected);
        // }
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