using AoC.Util;

var exampleInput =
    """
    Time:      7  15   30
    Distance:  9  40  200
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var times = lines[0].Split().Where(s => !s.IsNullOrWhitespace()).ToArray();
    var distances = lines[1].Split().Where(s => !s.IsNullOrWhitespace()).ToArray();
    return times
        .Indexed()
        .Skip(1)
        .Select(((int i, string time) tuple) =>
        {
            var nrOfWin = 0;
            for (int j = 1; j < tuple.time.ToInt(); j++)
            {
                var dist = (tuple.time.ToInt() - j) * j;
                if (dist > distances[tuple.i].ToInt())
                    nrOfWin++;
            }
            return nrOfWin;
        }).Where(result => result != 0)
        .Multiply()
        .ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var times = new[]{lines[0].Replace(" ", "").Replace("Time:", "")};
    var distances = new[] { lines[1].Replace(" ", "").Replace("Distance:", "")};
    return times
        .Indexed()
        .Select(((int i, string time) tuple) =>
        {
            var nrOfWin = 0;
            for (int j = 1; j < tuple.time.ToLong(); j++)
            {
                var dist = (tuple.time.ToLong() - j) * j;
                if (dist > distances[tuple.i].ToLong())
                    nrOfWin++;
            }
            return nrOfWin;
        }).Where(result => result != 0)
        .Multiply()
        .ToString();
}