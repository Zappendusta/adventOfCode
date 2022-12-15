using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace AdventOfCode.Test;

public class Directory
{
    public Directory(string location)
    {
        Location = location;
    }

    public string Location { get; }
    public List<string> SubDirectories { get; } = new();
    public List<int> Files { get; } = new();

    public void AddFile(int size)
    {
        Files.Add(size);
    }

    public void AddDir(string location)
    {
        SubDirectories.Add(location);
    }

    public int GetSize(IReadOnlyList<Directory> allDirectories)
    {
        var size = Files.Sum();
        size += SubDirectories
            .Select(subDirLocation => allDirectories
                .Single(subDir => subDir.Location == subDirLocation)
                .GetSize(allDirectories))
            .Sum();
        return size;
    }
}

public class Finder
{
    public Finder()
    {
        CurrentLocation = "";
        Directories.Add(new Directory(CurrentLocation));
    }

    private bool SkipUntilNextCommand { get; set; }

    public string CurrentLocation { get; private set; }
    public List<Directory> Directories { get; } = new();

    private Directory GetCurrentDir => Directories.Single(x => x.Location == CurrentLocation);

    public void AddDirectory(string newDirName)
    {
        if (GetCurrentDir.SubDirectories.Contains(newDirName))
        {
            SkipUntilNextCommand = true;
        }

        var newLocation = CurrentLocation + newDirName;
        GetCurrentDir.AddDir(CurrentLocation + newDirName);
        Directories.Add(new Directory(newLocation));
    }

    public void AddFileToDir(int file)
    {
        if (SkipUntilNextCommand) return;
        GetCurrentDir.AddFile(file);
    }

    public int Get100KFolderSizes()
    {
        return Directories.Select(x => x.GetSize(Directories)).Where(x => x <= 100000).Sum();
    }

    public int GetHomeSize()
    {
        return Directories.Single(x => x.Location == "").GetSize(Directories);
    }

    public void CdInto(string dir)
    {
        SkipUntilNextCommand = false;
        CurrentLocation += dir;
    }

    public void CdBack()
    {
        SkipUntilNextCommand = false;
        CurrentLocation = Directories.Single(x => x.SubDirectories.Contains(CurrentLocation)).Location;
    }

    public void CdHome()
    {
        SkipUntilNextCommand = false;
        CurrentLocation = "";
    }
}

public enum CommandType
{
    CdHome,
    CdBack,
    CdInto,
    Dir,
    File,
    Ls,
}

public class Day7
{
    private readonly ITestOutputHelper _output;
    private const int Day = 7;

    public Day7(ITestOutputHelper output)
    {
        _output = output;
    }

    private static CommandType GetCommandType(string input)
    {
        if (input == "$ cd /")
        {
            return CommandType.CdHome;
        }

        if (input == "$ cd ..")
        {
            return CommandType.CdBack;
        }

        if (input.StartsWith("$ cd"))
        {
            return CommandType.CdInto;
        }

        if (input.StartsWith("dir"))
        {
            return CommandType.Dir;
        }

        if (input == "$ ls")
        {
            return CommandType.Ls;
        }

        if (int.TryParse(input.Split(" ")[0], out _))
        {
            return CommandType.File;
        }

        throw new Exception();
    }

    private static string GetDirInto(string dirIntoCommand)
    {
        return dirIntoCommand.Split(" ")[2];
    }

    private static string GetDir(string newDirCommand)
    {
        return newDirCommand.Split(" ")[1];
    }

    private static int GetFileSize(string fileCommand)
    {
        return int.Parse(fileCommand.Split(" ")[0]);
    }

    private static Finder GetFinalFinder(IEnumerable<string> input)
    {
        var finder = new Finder();

        foreach (var command in input)
        {
            switch (GetCommandType(command))
            {
                case CommandType.CdHome:
                    finder.CdHome();
                    break;
                case CommandType.CdBack:
                    finder.CdBack();
                    break;
                case CommandType.CdInto:
                    var dirInto = GetDirInto(command);
                    finder.CdInto(dirInto);
                    break;
                case CommandType.Dir:
                    var newDir = GetDir(command);
                    finder.AddDirectory(newDir);
                    break;
                case CommandType.File:
                    var size = GetFileSize(command);
                    finder.AddFileToDir(size);
                    break;
                case CommandType.Ls:
                    // dont care
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return finder;
    }

    private static string Solve1(IEnumerable<string> input)
    {
        var finder = GetFinalFinder(input);

        return finder.Get100KFolderSizes().ToString();
    }


    private static string Solve2(IEnumerable<string> input)
    {
        var finder = GetFinalFinder(input);
        var max = 70000000;
        var need = 30000000;

        var all = finder.Directories.Select(notIncluded =>
        {
            var totalSize = finder.GetHomeSize();
            var folderSize = notIncluded.GetSize(finder.Directories);
            var sizeAfterDelete = totalSize - folderSize;
            var freeSpace = max - sizeAfterDelete;
            return new
            {
                totalSize,
                folderSize,
                sizeAfterDelete, 
                freeSpace,
            };
        }).ToList();

        var sortedList = all.OrderBy(x => x.freeSpace).Where(x => x.freeSpace >= need).ToList();
        var bestMatch = sortedList.First();
        
        return bestMatch.folderSize.ToString();
    }

    [Fact]
    public void TestSolveTest()
    {
        var input = Input.Get(Day);
        {
            var result = Solve1(input);
            const string expected = "95437";
            result.Should().Be(expected);
        }
        {
            var result = Solve2(input);
            const string expected = "24933642";
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