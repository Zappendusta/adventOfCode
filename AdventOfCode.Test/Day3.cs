using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day3
{
    private readonly ITestOutputHelper _output;
    private const int Day = 3;

    public Day3(ITestOutputHelper output)
    {
        _output = output;
    }


    private static char GetDuplicateType(string line)
    {
        var firstHalf = line.Take(line.Length / 2);
        var secondHalf = line.Skip(line.Length / 2);
        return firstHalf.Intersect(secondHalf).Single();
    }

    private static int CharToAlphabeticalInt(char input)
    {
        return char.IsUpper(input) ? (byte)input - (byte)'A' + 27 : (byte)input - (byte)'a' + 1;
    }

    private static string Solve1(IEnumerable<string> input)
    {
        return input.Select(GetDuplicateType).Select(CharToAlphabeticalInt).Sum().ToString();
    }

    private static char GetDuplicateItem(IReadOnlyList<string> input)
    {
        return input[0].Intersect(input[1]).Intersect(input[2]).Single();
    }

    private static string Solve2(IEnumerable<string> input)
    {
        var a = input.Partition(3).ToList();

        var b = a.Select(x => GetDuplicateItem(x.ToList())).ToList();
        var c = b.Select(x => x)
            .Select(CharToAlphabeticalInt)
            .Sum().ToString();
        return c;
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "157";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "70";
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

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> values, int chunkSize)
    {
        return values.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
}