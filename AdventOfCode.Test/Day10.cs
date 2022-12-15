using System.Collections;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public enum Instruction
{
    Noop,
    AddX,
}

public class Day10
{
    private readonly ITestOutputHelper _output;
    private const int Day = 10;

    public Day10(ITestOutputHelper output)
    {
        _output = output;
    }


    private string Solve1(IEnumerable<string> input)
    {
        var x = 1;
        var signalStrength = 0;
        var currentCycle = 0;
        var currentLine = "";

        void AdjustSignalStrength()
        {
            if ((currentCycle - 20) % 40 == 0)
            {
                signalStrength += x * currentCycle;
            }

            if ((currentCycle) % 40 == 0)
            {
                _output.WriteLine(currentLine);
                currentLine = "";
            }
        }

        void IncreaseCycle()
        {
            var crtPosition = (currentCycle) % 40;
            var lit = x - 1 <= crtPosition && x + 1 >= crtPosition;

            currentLine += lit ? "#" : ".";


            currentCycle += 1;
            AdjustSignalStrength();
        }

        foreach (var (instruction, amount) in input.Select(y =>
                 {
                     var spl = y.Split(" ");
                     return spl[0] switch
                     {
                         "noop" => (Instruction.Noop, 0),
                         // ReSharper disable once StringLiteralTypo
                         "addx" => (Instruction.AddX, int.Parse(spl[1])),
                         _ => throw new Exception()
                     };
                 }))
        {
            switch (instruction)
            {
                case Instruction.Noop:
                    IncreaseCycle();
                    break;
                case Instruction.AddX:
                    IncreaseCycle();
                    IncreaseCycle();
                    x += amount;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return signalStrength.ToString();
    }


    private string Solve2(IEnumerable<string> input)
    {
        return Solve1(input);
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "13140";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "13140";
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