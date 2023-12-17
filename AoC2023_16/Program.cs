using AoC.Util;

var exampleInput =
    """
    .|...\....
    |.-.\.....
    .....|-...
    ........|.
    ..........
    .........\
    ..../.\\..
    .-.-/..|..
    .|....-|.\
    ..//.|....
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    
    

    return SimulateBeam(lines, (0,0),3).ToString();
}

int SimulateBeam(string[] lines, (int x, int y) coord, int cameFrom)
{
    var energized = new HashSet<((int, int), int)>();
    void Beam((int x, int y) coord, int cameFrom)
    {
        if (!coord.y.IsBetween(0, lines.Length - 1) || !coord.x.IsBetween(0, lines[coord.y].Length - 1))
            return;
        if (!energized.Add((coord, cameFrom)))
            return;

        //for (int y = 0; y < lines.Length; y++)
        //{
        //    for (int x = 0; x < lines[y].Length; x++)
        //    {
        //        if (coord.x == x && coord.y == y)
        //            Console.BackgroundColor = ConsoleColor.Red;
        //        else
        //            Console.BackgroundColor = ConsoleColor.Black;
        //        Console.Write(energized.Any(tuple => tuple.Item1 == (x,y))?'#':'.');
        //    }
        //    Console.WriteLine();
        //}
        //Console.WriteLine();
        var tile = lines[coord.y][coord.x];
        switch (tile)
        {
            case '.':
                switch (cameFrom)
                {
                    case 0: Beam((coord.x, coord.y + 1), 0); break;
                    case 1: Beam((coord.x - 1, coord.y), 1); break;
                    case 2: Beam((coord.x, coord.y - 1), 2); break;
                    case 3: Beam((coord.x + 1, coord.y), 3); break;
                }
                break;
            case '-':
                switch (cameFrom)
                {
                    case 0: Beam((coord.x + 1, coord.y), 3); Beam((coord.x - 1, coord.y), 1); break;
                    case 1: Beam((coord.x - 1, coord.y), 1); break;
                    case 2: Beam((coord.x + 1, coord.y), 3); Beam((coord.x - 1, coord.y), 1); break;
                    case 3: Beam((coord.x + 1, coord.y), 3); break;
                }
                break;
            case '|':
                switch (cameFrom)
                {
                    case 0: Beam((coord.x, coord.y + 1), 0); break;
                    case 1: Beam((coord.x, coord.y - 1), 2); Beam((coord.x, coord.y + 1), 0); break;
                    case 2: Beam((coord.x, coord.y - 1), 2); break;
                    case 3: Beam((coord.x, coord.y - 1), 2); Beam((coord.x, coord.y + 1), 0); break;
                }
                break;
            case '/':
                switch (cameFrom)
                {
                    case 0: Beam((coord.x - 1, coord.y), 1); break;
                    case 1: Beam((coord.x, coord.y + 1), 0); break;
                    case 2: Beam((coord.x + 1, coord.y), 3); break;
                    case 3: Beam((coord.x, coord.y - 1), 2); break;
                }
                break;
            case '\\':
                switch (cameFrom)
                {
                    case 0: Beam((coord.x + 1, coord.y), 3); break;
                    case 1: Beam((coord.x, coord.y - 1), 2); break;
                    case 2: Beam((coord.x - 1, coord.y), 1); break;
                    case 3: Beam((coord.x, coord.y + 1), 0); break;
                }
                break;
        }
    }
    /**
     *  0
     * 3\1
     *  2
     */

    Beam(coord, cameFrom);

    return energized.DistinctBy(tuple => tuple.Item1).Count();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var best = 0;
    for (int x = 0; x < lines[0].Length; x++)
    {
        best = Math.Max(best, SimulateBeam(lines, (x, 0), 0));
        best = Math.Max(best, SimulateBeam(lines, (x, lines.Length-1), 2));
    }

    for (int y = 0; y < lines.Length; y++)
    {
        best = Math.Max(best, SimulateBeam(lines, (0, y), 3));
        best = Math.Max(best, SimulateBeam(lines, (lines[y].Length,y), 1));
    }
    return best.ToString();
}