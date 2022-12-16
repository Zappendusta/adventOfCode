using FluentAssertions;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public static class PrintClass
{
    public static void Clear()
    {
        Console.Clear();
    }

    public static void PrintNode(Node n)
    {
        if (!n.IsVisited) return;
        if (n.FromPosition is null) return;

        Console.SetCursorPosition(n.FromPosition.Item1, n.FromPosition.Item2);
        if (n.Position.Item1 - n.FromPosition.Item1 > 0) Console.Write('>');
        else if (n.Position.Item1 - n.FromPosition.Item1 < 0) Console.Write('<');
        else if (n.Position.Item2 - n.FromPosition.Item2 > 0) Console.Write('v');
        else if (n.Position.Item2 - n.FromPosition.Item2 < 0) Console.Write('^');
    }

    public static void PrintDot(Node n)
    {
        Console.SetCursorPosition(n.Position.Item1, n.Position.Item2);
        Console.Write('.');
    }

    public static void PrintNodeSign(Node n)
    {
        Console.SetCursorPosition(n.Position.Item1, n.Position.Item2);
        Console.Write(n.Sign);
    }
}

public class Node
{
    public char Sign { get; init; }
    public int Distance { get; set; } = int.MaxValue;
    public Tuple<int, int> Position { get; init; } = new(0, 0);
    public Tuple<int, int>? FromPosition { get; set; }
    public bool IsGoal { get; init; }
    public bool IsVisited { get; set; }
    public int Elevation { get; init; }
}

public enum RelativePosition
{
    Right,
    Left,
    Up,
    Down,
}

public class Day12
{
    private readonly ITestOutputHelper _output;
    private const int Day = 12;

    public Day12(ITestOutputHelper output)
    {
        _output = output;
    }


    private static IEnumerable<Tuple<int, int>> GetNeighbors(Node n)
    {
        return Enum.GetValues<RelativePosition>().Select(pos => pos switch
        {
            RelativePosition.Right => new Tuple<int, int>(n.Position.Item1 + 1, n.Position.Item2),
            RelativePosition.Left => new Tuple<int, int>(n.Position.Item1 - 1, n.Position.Item2),
            RelativePosition.Up => new Tuple<int, int>(n.Position.Item1, n.Position.Item2 + 1),
            RelativePosition.Down => new Tuple<int, int>(n.Position.Item1, n.Position.Item2 - 1),
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    private static IEnumerable<Node> GetNeighbors(Node n, IReadOnlyCollection<Node> nodes)
    {
        return GetNeighbors(n)
            .Select(pos => nodes.SingleOrDefault(x => Equals(x.Position, pos)))
            .Where(node => node != null)
            .Where(node => !node!.IsVisited)!;
    }

    private static void SetDistance(Node fromNode, Node toNode)
    {
        var elevationDifference = Math.Abs(fromNode.Elevation - toNode.Elevation);
        if (elevationDifference > 1) return;
        toNode.Distance = fromNode.Distance + 1;
        toNode.FromPosition = fromNode.Position;
    }

    private static void Dijkstra(IReadOnlyCollection<Node> nodes)
    {
        Node? GetNextNode()
        {
            return nodes
                .Where(x => !x.IsVisited)
                .Where(x => x.Distance < int.MaxValue)
                .MinBy(x => x.Distance);
        }

        while (true)
        {
            var fromNode = GetNextNode();
            if (fromNode is null) break;
            if (fromNode.IsGoal) break;
            fromNode.IsVisited = true;

            foreach (var toNode in GetNeighbors(fromNode, nodes))
            {
                SetDistance(fromNode, toNode);
            }
        }
    }

    private static List<Node> InputToNodes(IEnumerable<string> input)
    {
        var nodes = new List<Node>();
        foreach (var (item, i) in input.WithIndex())
        {
            foreach (var (cell, j) in item.WithIndex())
            {
                nodes.Add(new Node
                {
                    Distance = cell == 'S' ? 0 : int.MaxValue,
                    Position = new Tuple<int, int>(j, i),
                    IsGoal = cell == 'E',
                    Elevation = cell switch
                    {
                        'S' => 0,
                        'E' => 27,
                        _ => (byte)cell - (byte)'a' + 1
                    },
                    Sign = cell
                });
            }
        }

        return nodes;
    }

    private static string Solve1(IEnumerable<string> input)
    {
        var nodes = InputToNodes(input);
        Dijkstra(nodes);

        PrintClass.Clear();
        nodes.ForEach(PrintClass.PrintDot);
        nodes.ForEach(PrintClass.PrintNode);

        PrintClass.Clear();
        nodes.ForEach(PrintClass.PrintDot);

        var last = nodes.Single(x => x.IsGoal);
        while (last.Elevation > 0)
        {
            PrintClass.PrintNode(last);
            last = nodes.Single(x => Equals(x.Position, last.FromPosition));
        }

        return nodes.Single(x => x.IsGoal).Distance.ToString();
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
            const string expected = "31";
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