using AoC.Util;

var exampleInput =
    """
    ..F7.
    .FJ|.
    SJ.L7
    |F--J
    LJ...
    """;

var exampleInput2 =
    """
    .F----7F7F7F7F-7....
    .|F--7||||||||FJ....
    .||.FJ||||||||L7....
    FJL7L7LJLJ||LJ.L-7..
    L--J.L7...LJS7F-7L7.
    ....F-J..F7FJ|L7L7L7
    ....L7.F7||L7|.L7L7|
    .....|FJLJ|FJ|F7|.LJ
    ....FJL-7.||.||||...
    ....L---J.LJ.LJLJ...
    """;
//  .┌────┐┌┐┌┐┌┐┌─┐....
//  .│┌──┐││││││││┌┘....
//  .││.┌┘││││││││└┐....
//  ┌┘└┐└┐└┘└┘││└┘.└─┐..
//  └──┘.└┐...└┘S┐┌─┐└┐.
//  ....┌─┘..┌┐┌┘│└┐└┐└┐
//  ....└┐.┌┐││└┐│.└┐└┐│
//  .....│┌┘└┘│┌┘│┌┐│.└┘
//  ....┌┘└─┐.││.││││...
//  ....└───┘.└┘.└┘└┘...

var exampleInput3 =
    """
    ...........
    .S-------7.
    .|F-----7|.
    .||.....||.
    .||.....||.
    .|L-7.F-J|.
    .|..|.|..|.
    .L--J.L--J.
    ...........
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput2)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput2)}");

(int, int)[] FindPipePath(string input)
{
    var lines = input.Lines();
    var start = (0, 0);
    for (int y = 0; y < lines.Length; y++)
    {
        int x;
        if ((x = lines[y].IndexOf('S')) != -1)
        {
            start = (x, y);
        }
    }

    var pipes = new List<(int, int)>();
    var cameFrom = start;
    var current = start;
    while(true)
    {
        pipes.Add(current);
        var curPipe = lines[current.Item2][current.Item1];
        var nextPipes = curPipe switch
        {
            'F' => new[] { current.Add((1, 0)), current.Add((0, 1)) },
            '-' => new[] { current.Add((1, 0)), current.Add((-1, 0)) },
            '7' => new[] { current.Add((-1, 0)), current.Add((0, 1)) },
            '|' => new[] { current.Add((0, -1)), current.Add((0, 1)) },
            'J' => new[] { current.Add((-1, 0)), current.Add((0, -1)) },
            'L' => new[] { current.Add((1, 0)), current.Add((0, -1)) },
            'S' => ((Func<(int, int)[]>)(() =>
            {
                // Missing edge-cases for start near edge
                var nexts = new List<(int, int)>();
                if ("F7|".Contains(lines[current.Item2 - 1][current.Item1]))
                    nexts.Add(current.Add((0, -1)));
                if ("-7J".Contains(lines[current.Item2][current.Item1 + 1]))
                    nexts.Add(current.Add((1, 0)));
                if ("JL|".Contains(lines[current.Item2 + 1][current.Item1]))
                    nexts.Add(current.Add((0, 1)));
                if ("-FL".Contains(lines[current.Item2][current.Item1 - 1]))
                    nexts.Add(current.Add((-1, 0)));

                return nexts.ToArray();
            }))()
        };
        var nextPipe = nextPipes.First(tuple => tuple != cameFrom);
        if (nextPipe == start)
            return pipes.ToArray();
        cameFrom = current;
        current = nextPipe;
    }
}

string Solve1(string input)
{
    var pipes = FindPipePath(input);
    return (pipes.Length/2).ToString();
}

string Solve2(string input)
{
    var pipes = FindPipePath(input);
    var lines = input.Lines();
    var insides = 0;
    for (int y = 1; y < lines.Length-1; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            if(pipes.Contains((x,y)))
                continue;
            var windings = 0; // https://en.wikipedia.org/wiki/Nonzero-rule
            for (int i = x; i < lines[y].Length; i++)
            {
                if (pipes.Contains((i, y)))
                {
                    var pipeIndex = pipes.IndexOf((i, y));
                    if (pipes[(pipeIndex + 1) % pipes.Length] == (i, y + 1))
                        windings++;
                    if (pipes[(pipeIndex - 1 + pipes.Length) % pipes.Length] == (i, y + 1))
                        windings--;
                }
            }

            if (windings != 0)
                insides++;
        }
    }
    return insides.ToString();
}