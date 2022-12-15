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
    public int Height { get;  }
    public bool Visible { get; set; }
}

public class Forest
{
    public int Width => Rows[0].Count;
    public int Height => Rows.Count;
    public List<List<Tree>> Rows { get; set; } = new List<List<Tree>>();

    public List<List<Tree>> Columns()
    {
        var columns = new List<List<Tree>>();
        for (var i = 0; i < Width; i++)
        {
            columns.Add(new List<Tree>());
            for (var j = 0; j < Height; j++)
            {
                columns[i].Add(Rows[Width][Height]);
            }
        }
        return columns;
    }

    public override string ToString()
    {
        return Rows.Aggregate("", (current, row) => current + string.Join("", row.Select(x => x.Height.ToString()))+"\n");
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

        foreach (var (row,index) in input.WithIndex())
        {
            forest.Rows.Add(new List<Tree>());
            foreach (var tree in row)
            {
                forest.Rows[index].Add(new Tree(int.Parse(tree.ToString())));
            }
        }
        
        
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
            const string expected = "21";
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

public static class Extension
{
    
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
}