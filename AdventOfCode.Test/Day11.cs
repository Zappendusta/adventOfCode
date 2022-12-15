using System.Collections;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Monkey
{
    public long Id { get; private init; }
    public List<long> Items { get; set; }
    public long TimesInspected { get; set; }
    public long Divisor { get; set; }
    public Func<long, long> ThrowItemTo { get; set; }
    public Func<long, long> InspectItem { get; set; }


    public static Monkey Parse(IReadOnlyList<string> data)
    {
        var testDivisor = ParseTest(data.Skip(3).ToList());
        return new Monkey()
        {
            Id = long.Parse(data[0].Split(" ")[1][0].ToString()),
            Items = ParseItems(data[1]).ToList(),
            InspectItem = ParseOperation(data[2]),
            ThrowItemTo = testDivisor.test,
            Divisor = testDivisor.divisibleBy,
        };
    }

    private static Func<long, long> ParseOperation(string input)
    {
        var splits = input.Split(" ");
        var isNumber = long.TryParse(splits.AsEnumerable().Reverse().Take(1).Single(), out var amount);

        if (!isNumber)
        {
        }

        return splits.AsEnumerable().Reverse().Skip(1).Take(1).Single() switch
        {
            "*" => oldItem => oldItem * (isNumber ? amount : oldItem),
            "+" => oldItem => oldItem + (isNumber ? amount : oldItem),
            _ => throw new Exception()
        };
    }

    private static IEnumerable<long> ParseItems(string input)
    {
        return input.Split(" ").SelectMany(x => x.Split(",")).Where(x => long.TryParse(x, out _)).Select(long.Parse);
    }

    private static (Func<long, long> test,long divisibleBy) ParseTest(IReadOnlyList<string> input)
    {
        var divisibleBy = long.Parse(input[0].Split(" ").Last());
        var ifTrue = long.Parse(input[1].Split(" ").Last());
        var ifFalse = long.Parse(input[2].Split(" ").Last());
        return (i => i % divisibleBy == 0 ? ifTrue : ifFalse, divisibleBy);
    }
}

public class Day11
{
    private readonly ITestOutputHelper _output;
    private const int Day = 11;

    public Day11(ITestOutputHelper output)
    {
        _output = output;
    }


    private static string Solve1(IEnumerable<string> input)
    {
        var monkeys = input.Chunk(7).Select(Monkey.Parse).ToList();

        for (var i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys.OrderBy(x => x.Id))
            {
                foreach (var item in monkey.Items)
                {
                    var newWorryLevel = monkey.InspectItem.Invoke(item);
                    monkey.TimesInspected++;
                    newWorryLevel = (long)Math.Floor(newWorryLevel / 3.0);
                    var throwToMonkey = monkey.ThrowItemTo.Invoke(newWorryLevel);
                    monkeys.Single(x => x.Id == throwToMonkey).Items.Add(newWorryLevel);
                    monkey.Items = new List<long>();
                }
            }
        }

        return monkeys.OrderByDescending(x => x.TimesInspected)
            .Take(2)
            .Select(x => x.TimesInspected)
            .Aggregate((a, x) => a * x)
            .ToString();
    }


    private static string Solve2(IEnumerable<string> input)
    {
        var monkeys = input.Chunk(7).Select(Monkey.Parse).ToList();
        var factor = monkeys.Select(x => x.Divisor).Aggregate((a, b) => a * b);

        for (var i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var item in monkey.Items)
                {
                    var newWorryLevel = monkey.InspectItem.Invoke(item);
                    monkey.TimesInspected++;
                    if (newWorryLevel != factor && factor != 0)
                    {
                        newWorryLevel = newWorryLevel % factor;
                    }

                    var throwToMonkey = monkey.ThrowItemTo.Invoke(newWorryLevel);
                    monkeys.Single(x => x.Id == throwToMonkey).Items.Add(newWorryLevel);
                    monkey.Items = new List<long>();
                }
            }
        }

        return monkeys.OrderByDescending(x => x.TimesInspected)
            .Take(2)
            .Select(x => x.TimesInspected)
            .Aggregate((a, x) => a * x)
            .ToString();
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "10605";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "2713310158";
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