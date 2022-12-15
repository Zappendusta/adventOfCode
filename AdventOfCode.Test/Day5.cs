using System.Collections;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Day5
{
    private readonly ITestOutputHelper _output;
    private const int Day = 5;

    public Day5(ITestOutputHelper output)
    {
        _output = output;
    }

    private static void Move(IReadOnlyList<Stack<char>> stacks, int times, int from, int to, bool canMoveMultiple)
    {
        if (canMoveMultiple)
        {
            var midStack = new Stack<char>();
            for (var i = 0; i < times; i++)
            {
                var crate = stacks[from].Pop();
                midStack.Push(crate);
            }

            for (var i = 0; i < times; i++)
            {
                var crate = midStack.Pop();
                stacks[to].Push(crate);
            }
        }
        else
        {
            for (var i = 0; i < times; i++)
            {
                var crate = stacks[from].Pop();
                stacks[to].Push(crate);
            }
        }
    }


    private static void AddCrate(IReadOnlyList<Stack<char>> stacks, int to, char crate)
    {
        if (crate == ' ') return;
        stacks[to].Push(crate);
    }

    private static char CharOfPosition(string level, int position)
    {
        return level[1 + position * 4];
    }

    private static List<int> ExtractMoveInfo(string move)
    {
        return move.Split(" ").Where(x => int.TryParse(x.Trim(), out _)).Select(int.Parse).ToList();
    }

    private static string Solve1(IEnumerable<string> input)
    {
        var inputEnumerated = input.ToList();
        var crates = inputEnumerated.TakeWhile(x => !string.IsNullOrEmpty(x)).Reverse().ToList();
        var stackAmount = crates[0].Where(x => int.TryParse(x.ToString(), out _)).Select(x => x.ToString())
            .Select(int.Parse)
            .Max();
        var stacks = Enumerable.Range(0, stackAmount).Select(_ => new Stack<char>()).ToList();

        foreach (var level in crates.Skip(1))
        {
            for (var position = 0; position < stackAmount; position++)
            {
                AddCrate(stacks, position, CharOfPosition(level, position));
            }
        }

        var moves = inputEnumerated.Where(x => x.StartsWith("move")).ToList();

        moves.ForEach(x =>
        {
            var moveInfo = ExtractMoveInfo(x);
            Move(stacks, moveInfo[0], moveInfo[1] - 1, moveInfo[2] - 1, false);
        });

        return string.Join("", stacks.Select(x => x.Pop()));
    }

    private static string Solve2(IEnumerable<string> input)
    {
        var inputEnumerated = input.ToList();
        var crates = inputEnumerated.TakeWhile(x => !string.IsNullOrEmpty(x)).Reverse().ToList();
        var stackAmount = crates[0].Where(x => int.TryParse(x.ToString(), out _)).Select(x => x.ToString())
            .Select(int.Parse)
            .Max();
        var stacks = Enumerable.Range(0, stackAmount).Select(_ => new Stack<char>()).ToList();

        foreach (var level in crates.Skip(1))
        {
            for (var position = 0; position < stackAmount; position++)
            {
                AddCrate(stacks, position, CharOfPosition(level, position));
            }
        }

        var moves = inputEnumerated.Where(x => x.StartsWith("move")).ToList();

        moves.ForEach(x =>
        {
            var moveInfo = ExtractMoveInfo(x);
            Move(stacks, moveInfo[0], moveInfo[1] - 1, moveInfo[2] - 1, true);
        });

        return string.Join("", stacks.Select(x => x.Pop()));
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "CMZ";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "MCD";
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