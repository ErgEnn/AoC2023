using AoC.Util;

var exampleInput =
    """
    0 3 6 9 12 15
    1 3 6 10 15 21
    10 13 16 21 30 45
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    return lines.Select(line =>
    {
        var numbers = line.Split().ToInts().ToList();
        var subnumbers = new List<List<int>> { numbers };
        while (true)
        {
            var nextNos = subnumbers.Last().Pairwise().Select(tuple => tuple.i2 - tuple.i1).ToList();
            if (nextNos.All(i => i == 0))
            {
                break;
            }
            subnumbers.Add(nextNos);
        }

        for (int i = subnumbers.Count - 2; i >= 0; i--)
        {
            subnumbers[i].Add(subnumbers[i].Last()+subnumbers[i+1].Last());
        }

        return subnumbers[0].Last();
    }).Sum().ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    return lines.Select(line =>
    {
        var numbers = line.Split().ToInts().ToList();
        var subnumbers = new List<List<int>> { numbers };
        while (true)
        {
            var nextNos = subnumbers.Last().Pairwise().Select(tuple => tuple.i2 - tuple.i1).ToList();
            if (nextNos.All(i => i == 0))
            {
                break;
            }
            subnumbers.Add(nextNos);
        }

        for (int i = subnumbers.Count - 2; i >= 0; i--)
        {
            subnumbers[i].Insert(0,subnumbers[i].First() - subnumbers[i + 1].First());
        }

        return subnumbers[0].First();
    }).Sum().ToString();
}