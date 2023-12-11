using AoC.Util;

var exampleInput =
    """
    ...#......
    .......#..
    #.........
    ..........
    ......#...
    .#........
    .........#
    ..........
    .......#..
    #...#.....
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var colMask = new bool[lines[0].Length];
    var rowMask = new bool[lines.Length];
    var galaxies = new List<(int, int)>();
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            var chr = lines[y][x];
            if (chr == '#')
            {
                galaxies.Add((x,y));
                colMask[x] = true;
                rowMask[y] = true;
            }
        }
    }

    long sum = 0;
    foreach (((int x, int y) g1, (int x, int y) g2)  in galaxies.AllPairs())
    {
        sum+=
            Math.Abs(g1.x - g2.x)
            + Math.Abs(g1.y - g2.y)
            + colMask[(g1.x..g2.x).ToAbs()].Count(b => !b)
            + rowMask[(g1.y..g2.y).ToAbs()].Count(b => !b);
    }
    return sum.ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var colMask = new bool[lines[0].Length];
    var rowMask = new bool[lines.Length];
    var galaxies = new List<(int, int)>();
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            var chr = lines[y][x];
            if (chr == '#')
            {
                galaxies.Add((x, y));
                colMask[x] = true;
                rowMask[y] = true;
            }
        }
    }
    PrintMap(colMask,rowMask,galaxies,10);
    long sum = 0;
    foreach (((int x, int y) g1, (int x, int y) g2) in galaxies.AllPairs())
    {
        sum+=
            Math.Abs(g1.x - g2.x)
            + Math.Abs(g1.y - g2.y)
            + colMask[(g1.x..g2.x).ToAbs()].Count(b => !b) * 999_999
            + rowMask[(g1.y..g2.y).ToAbs()].Count(b => !b) * 999_999;
    }
    return sum.ToString();
}

void PrintMap(bool[] colMask, bool[] rowMask, List<(int,int)> galaxies, int multiplier = 2)
{
    for (int y = 0; y < rowMask.Length; y++)
    {
        if (rowMask[y])
        {
            for (int x = 0; x < colMask.Length; x++)
            {
                if (galaxies.IndexOf((x, y)) != -1)
                    Console.Write(galaxies.IndexOf((x, y))+1);
                else if (!colMask[x])
                    Console.Write(string.Join("", Enumerable.Repeat(".", multiplier)));
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }
        else
            for (int i = 0; i < multiplier; i++)
            {
                Console.WriteLine(string.Join("", Enumerable.Repeat(".", colMask.Select(b => b?1:multiplier).Sum())));
            }
        
    }
}