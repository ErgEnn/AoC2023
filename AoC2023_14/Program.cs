using AoC.Util;
using AoC2023_14;

var exampleInput =
    """
    O....#....
    O.OO#....#
    .....##...
    OO.#O....O
    .O.....O#.
    O.#..O.#.#
    ..O..#O..O
    .......O..
    #....###..
    #OO..#....
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    return lines.Transpose().FallLeft().Select(column =>
    {
        var sum = 0;
        for (int i = 0; i < column.Length; i++)
        {
            if (column[i] == 'O')
                sum += column.Length - i;
        }

        return sum;
    }).Sum().ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();

    var current = lines;

    var dict = new Dictionary<string, (long,string[])>();
    var cycles = 1000000000;
    for (long i = 0; i < cycles; i++)
    {
        current = current.Transpose()
            .FallLeft()
            .Transpose()
            .FallLeft()
            .Transpose()
            .Select(row => row.Reverse().Join(""))
            .FallLeft()
            .Transpose()
            .Select(row => row.Reverse().Join(""))
            .FallLeft()
            .Select(row => row.Reverse().Join(""))
            .Reverse()
            .ToArray();

        if (dict.TryGetValue(current.Join(""), out var existing))
        {
            current = dict.Values.Single(tuple => tuple.Item1 == existing.Item1 + (((cycles-1) - i) % (i - existing.Item1))).Item2;
            break;
        }
        dict.Add(current.Join(""), (i, current));

    }

    return current.Transpose().Select(column =>
    {
        var sum = 0;
        for (int i = 0; i < column.Length; i++)
        {
            if (column[i] == 'O')
                sum += column.Length - i;
        }

        return sum;
    }).Sum().ToString();
}