using AoC.Util;

var exampleInput =
    """
    R 6 (#70c710)
    D 5 (#0dc571)
    L 2 (#5713f0)
    D 2 (#d2c081)
    R 2 (#59c680)
    D 2 (#411b91)
    L 5 (#8ceee2)
    U 2 (#caa173)
    L 1 (#1b58a2)
    U 2 (#caa171)
    R 2 (#7807d2)
    U 3 (#a77fa3)
    L 2 (#015232)
    U 2 (#7a21e3)
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var current = (0, 0);
    var vertices = new List<(int, int)>();
    vertices.Add(current);
    var minY = 0;
    var maxY = 0;
    var minX = 0;
    var maxX = 0;
    foreach (var line in lines)
    {
        var (dir, steps, untrimmedHex) = line.Deconstruct<string, int, string>();
        var next = current;
        switch (dir)
        {
            case "U": next = current.Add((0, -steps)); break;
            case "D": next = current.Add((0, steps)); break;
            case "L": next = current.Add((-steps, 0)); break;
            case "R": next = current.Add((steps, 0)); break;
        }
        current = next;
        minY = Math.Min(minY, current.Item2);
        maxY = Math.Max(maxY, current.Item2);
        minX = Math.Min(minX, current.Item1);
        maxX = Math.Max(maxX, current.Item1);
        vertices.Add(current);
    }

    vertices = vertices.Select(tuple => tuple.Subtract((minX, minY))).ToList();
    var height = Math.Abs(minY) + Math.Abs(maxY);
    var area = 0;
    var boundary = 0;
    foreach (((int x, int y) a, (int x, int y) b)  in vertices.Pairwise(loop:true))
    {
        //foreach (var i in (a.x != b.x ? a.x : a.y).StepTowards((a.x != b.x ? b.x : b.y)))
        //{
            
        //    Console.CursorTop = a.y!=b.y ? i : a.y;
        //    Console.CursorLeft = a.x!=b.x ? i : a.x;
        //    Console.Write('#');
        //}

        if (a.x != b.x)
        {
            if (a.x < b.x)
                area += (height-a.y) * (b.x - a.x);
            else
                area -= (height-a.y) * (a.x - b.x);
        }

        boundary += Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    var interiorArea = area + 1 - (boundary / 2); //Pick's theorem
    return (interiorArea+boundary).ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var current = (0, 0);
    var vertices = new List<(int, int)>();
    vertices.Add(current);
    var minY = 0;
    var maxY = 0;
    var minX = 0;
    var maxX = 0;
    foreach (var line in lines)
    {
        var (_, _, untrimmedHex) = line.Deconstruct<string, int, string>();
        var steps = ("0x"+untrimmedHex[2..^2]).ToInt(16);
        var dir = untrimmedHex[^2];
        var next = current;
        switch (dir)
        {
            case '3': next = current.Add((0, -steps)); break;
            case '1': next = current.Add((0, steps)); break;
            case '2': next = current.Add((-steps, 0)); break;
            case '0': next = current.Add((steps, 0)); break;
        }
        current = next;
        minY = Math.Min(minY, current.Item2);
        maxY = Math.Max(maxY, current.Item2);
        minX = Math.Min(minX, current.Item1);
        maxX = Math.Max(maxX, current.Item1);
        vertices.Add(current);
    }

    vertices = vertices.Select(tuple => tuple.Subtract((minX, minY))).ToList();
    var height = Math.Abs(minY) + Math.Abs(maxY);
    var area = 0L;
    var boundary = 0;
    foreach (((int x, int y) a, (int x, int y) b) in vertices.Pairwise(loop: true))
    {
        if (a.x != b.x)
        {
            if (a.x < b.x)
                area += (long)(height - a.y) * (long)(b.x - a.x);
            else
                area -= (long)(height - a.y) * (long)(a.x - b.x);
        }

        boundary += Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    var interiorArea = area + 1 - (boundary / 2); //Pick's theorem
    return (interiorArea + boundary).ToString();
}