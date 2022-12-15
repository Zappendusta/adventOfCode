using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Tree
{
    public Tree(int height)
    {
        Height = height;
    }

    public int Height { get; }
    public bool Visible { get; private set; }
    public int ScenicScore { get; private set; } = 1;

    public void See()
    {
        Visible = true;
    }

    public void AddDistance(int distance)
    {
        ScenicScore *= distance;
    }
}

public class Forest
{
    private int Width => Trees[0].Count;
    private int Height => Trees.Count;
    private List<List<Tree>> Trees { get; } = new();

    private IEnumerable<IEnumerable<Tree>> Columns()
    {
        for (var i = 0; i < Width; i++)
        {
            yield return Column(i);
        }
    }

    private IEnumerable<Tree> Column(int i)
    {
        for (var j = 0; j < Height; j++)
        {
            yield return Trees[j][i];
        }
    }

    private IEnumerable<IEnumerable<Tree>> Rows()
    {
        for (var i = 0; i < Height; i++)
        {
            yield return Row(i);
        }
    }

    private IEnumerable<Tree> Row(int i)
    {
        for (var j = 0; j < Width; j++)
        {
            yield return Trees[i][j];
        }
    }

    public string RowsToString()
    {
        return Rows().Aggregate("",
            (current, row) => current + string.Join("", row.Select(x => x.Height.ToString())) + "\n");
    }

    public string ColsToString()
    {
        return Columns().Aggregate("",
            (current, row) => current + string.Join("", row.Select(x => x.Height.ToString())) + "\n");
    }

    public int CountVisible()
    {
        return Rows().SelectMany(x => x).Count(x => x.Visible);
    }

    public int HighestScore()
    {
        return Trees.SelectMany(x => x).MaxBy(x => x.ScenicScore)!.ScenicScore;
    }

    public void PlantTrees(IEnumerable<string> input)
    {
        foreach (var (row, index) in input.WithIndex())
        {
            Trees.Add(new List<Tree>());
            foreach (var tree in row)
            {
                Trees[index].Add(new Tree(int.Parse(tree.ToString())));
            }
        }

        MarkVisibleTrees();
        SetScenicScores();
    }

    private void MarkVisibleTrees()
    {
        foreach (var row in Rows())
        {
            MarkVisibleTrees(row);
            MarkVisibleTrees(row.Reverse());
        }

        foreach (var column in Columns())
        {
            MarkVisibleTrees(column);
            MarkVisibleTrees(column.Reverse());
        }
    }

    private void MarkVisibleTrees(IEnumerable<Tree> treesInALine)
    {
        var biggest = int.MinValue;
        foreach (var tree in treesInALine)
        {
            if (tree.Height <= biggest)
            {
                continue;
            }

            biggest = tree.Height;
            tree.See();
        }
    }

    private void SetScenicScores()
    {
        for (var i = 1; i < Width - 1; i++)
        {
            for (var j = 1; j < Height - 1; j++)
            {
                var currentTree = Trees[i][j];

                // look left
                var steps = 0;
                while (true)
                {
                    steps++;
                    if (j - steps < 0)
                    {
                        steps--;
                        break;
                    }

                    var otherTree = Trees[i][j - steps];
                    if (otherTree.Height >= currentTree.Height) break;
                }

                currentTree.AddDistance(steps);


                // look right
                steps = 0;
                while (true)
                {
                    steps++;
                    if (j + steps >= Width)
                    {
                        steps--;
                        break;
                    }

                    var otherTree = Trees[i][j + steps];
                    if (otherTree.Height >= currentTree.Height) break;
                }

                currentTree.AddDistance(steps);


                // look up
                steps = 0;
                while (true)
                {
                    steps++;
                    if (i - steps < 0)
                    {
                        steps--;
                        break;
                    }

                    var otherTree = Trees[i - steps][j];
                    if (otherTree.Height >= currentTree.Height) break;
                }

                currentTree.AddDistance(steps);

                // look down
                steps = 0;
                while (true)
                {
                    steps++;
                    if (i + steps >= Height)
                    {
                        steps--;
                        break;
                    }

                    var otherTree = Trees[i + steps][j];
                    if (otherTree.Height >= currentTree.Height) break;
                }

                currentTree.AddDistance(steps);
            }
        }
    }
}

public class Day8
{
    private readonly ITestOutputHelper _output;
    private const int Day = 8;

    public Day8(ITestOutputHelper output)
    {
        _output = output;
    }

    private static string Solve1(IEnumerable<string> input)
    {
        var forest = new Forest();
        forest.PlantTrees(input);
        return forest.CountVisible().ToString();
    }


    private static string Solve2(IEnumerable<string> input)
    {
        var forest = new Forest();
        forest.PlantTrees(input);
        return forest.HighestScore().ToString();
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "21";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "8";
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

public static class Extension
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
}