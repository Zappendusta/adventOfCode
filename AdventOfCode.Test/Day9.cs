using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class Rope
{
    public Rope(int tailAmount)
    {
        Tails = Enumerable.Range(0, tailAmount + 1).Select(_ => new Position()).ToList();
    }

    private List<Position> TailVisitedPositions { get; } = new()
    {
        new Position()
    };

    private List<Position> Tails { get; }

    public int VisitedFieldCount()
    {
        return TailVisitedPositions.DistinctBy(x => $"{x.X}/{x.Y}").Count();
    }

    private (int x, int y) GetDir(Position from, Position to)
    {
        return (from.X - to.X, from.Y - to.Y);
    }

    private void AddCurrentTailPosition()
    {
        var t = Tails.Last();
        TailVisitedPositions.Add(new Position
        {
            X = Tails.Last().X,
            Y = Tails.Last().Y,
        });
    }

    public void MoveHeadHorizontally(int i)
    {
        for (var j = 0; j < Math.Abs(i); j++)
        {
            switch (i)
            {
                case > 0:
                    MoveRight();
                    TailFollows();
                    break;
                case < 0:
                    MoveLeft();
                    TailFollows();
                    break;
            }
        }
    }

    private void TailFollows()
    {
        for (var i = 1; i < Tails.Count; i++)
        {
            TailFollows(Tails[i - 1], Tails[i]);
        }
    }

    private void TailFollows(Position head, Position tail)
    {
        var direction = GetDir(head, tail);

        switch (direction)
        {
            case { x: 2, y: 2 }:
                tail.X += 1;
                tail.Y += 1;

                AddCurrentTailPosition();
                return;
            case { x: 2, y: -2 }:
                tail.X += 1;
                tail.Y -= 1;

                AddCurrentTailPosition();
                return;
            case { x: -2, y: 2 }:
                tail.X -= 1;
                tail.Y += 1;

                AddCurrentTailPosition();
                return;
            case { x: -2, y: -2 }:
                tail.X -= 1;
                tail.Y -= 1;

                AddCurrentTailPosition();
                return;


            case { x: 1, y: 2 }:
                tail.X += 1;
                tail.Y += 1;

                AddCurrentTailPosition();
                return;
            case { x: -1, y: 2 }:
                tail.X -= 1;
                tail.Y += 1;

                AddCurrentTailPosition();
                return;
            case { x: 1, y: -2 }:
                tail.X += 1;
                tail.Y -= 1;

                AddCurrentTailPosition();
                return;
            case { x: -1, y: -2 }:
                tail.X -= 1;
                tail.Y -= 1;

                AddCurrentTailPosition();
                return;


            case { x: 2, y: 1 }:
                tail.X += 1;
                tail.Y += 1;

                AddCurrentTailPosition();
                return;
            case { x: 2, y: -1 }:
                tail.X += 1;
                tail.Y -= 1;

                AddCurrentTailPosition();
                return;
            case { x: -2, y: 1 }:
                tail.X -= 1;
                tail.Y += 1;

                AddCurrentTailPosition();
                return;
            case { x: -2, y: -1 }:
                tail.X -= 1;
                tail.Y -= 1;

                AddCurrentTailPosition();
                return;


            default:


                switch (direction.x)
                {
                    case > 1:
                        tail.X += 1;
                        AddCurrentTailPosition();
                        return;
                    case < -1:
                        tail.X -= 1;
                        AddCurrentTailPosition();
                        return;
                }

                switch (direction.y)
                {
                    case > 1:
                        tail.Y += 1;
                        AddCurrentTailPosition();
                        return;
                    case < -1:
                        tail.Y -= 1;
                        AddCurrentTailPosition();
                        return;
                }

                break;
        }
    }

    public void MoveHeadVertically(int i)
    {
        for (var j = 0; j < Math.Abs(i); j++)
        {
            switch (i)
            {
                case > 0:
                    MoveUp();
                    TailFollows();
                    break;
                case < 0:
                    MoveDown();
                    TailFollows();
                    break;
            }
        }
    }

    private void MoveUp()
    {
        Tails[0].Y += 1;
    }

    private void MoveDown()
    {
        Tails[0].Y -= 1;
    }

    private void MoveLeft()
    {
        Tails[0].X -= 1;
    }

    private void MoveRight()
    {
        Tails[0].X += 1;
    }
}

public class Day9
{
    private readonly ITestOutputHelper _output;
    private const int Day = 9;

    public Day9(ITestOutputHelper output)
    {
        _output = output;
    }


    private static string SolveWith(IEnumerable<string> input, int i)
    {
        var rope = new Rope(i);
        foreach (var move in input.Select(x =>
                 {
                     var asd = x.Split(" ");
                     return new
                     {
                         dir = asd[0][0],
                         amount = int.Parse(asd[1])
                     };
                 }))
        {
            switch (move.dir)
            {
                case 'U':
                    rope.MoveHeadVertically(move.amount);
                    break;
                case 'D':
                    rope.MoveHeadVertically(move.amount * -1);
                    break;
                case 'L':
                    rope.MoveHeadHorizontally(move.amount * -1);
                    break;
                case 'R':
                    rope.MoveHeadHorizontally(move.amount);
                    break;
            }
        }

        return rope.VisitedFieldCount().ToString();
    }

    private static string Solve1(IEnumerable<string> input)
    {
        return SolveWith(input, 1);
    }


    private static string Solve2(IEnumerable<string> input)
    {
        return SolveWith(input, 9);
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "13";
            result.Should().Be(expected);
        }
        var input2 = Input.Get(Day, true, true);
        {
            var result = Solve2(input2);
            const string expected = "36";
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