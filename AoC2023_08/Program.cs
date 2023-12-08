using AoC.Util;

var exampleInput =
    """
    LR
    
    11A = (11B, XXX)
    11B = (XXX, 11Z)
    11Z = (11B, XXX)
    22A = (22B, XXX)
    22B = (22C, 22C)
    22C = (22Z, 22Z)
    22Z = (22B, 22B)
    XXX = (XXX, XXX)
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var steps = lines[0];
    var map = new Dictionary<string, (string, string)>();
    foreach (var line in lines.Skip(2))
    {
        var (loc, options) = line.Deconstruct<string, string>(" = ");
        var (l, r) = options.Trim('(', ')').Replace(" ", "").Deconstruct<string, string>(",");
        map.Add(loc,(l,r));
    }

    var current = map["AAA"];
    var noOfSteps = 0;
    for (int i = 0; true; i++)
    {
        var step = steps[i%steps.Length];
        var next = step switch
        {
            'L' => current.Item1,
            'R' => current.Item2,
        };
        if (next == "ZZZ")
        {
            noOfSteps = i + 1;
            break;
        }

        current = map[next];
    }
    return noOfSteps.ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var steps = lines[0];
    var map = new Dictionary<string, (string, string)>();
    foreach (var line in lines.Skip(2))
    {
        var (loc, options) = line.Deconstruct<string, string>(" = ");
        var (l, r) = options.Trim('(', ')').Replace(" ", "").Deconstruct<string, string>(",");
        map.Add(loc, (l, r));
    }

    IEnumerable<(string, int)> Walker((string, string ) start)
    {
        var current = start;
        for (int i = 0; true; i++)
        {
            var step = steps[i % steps.Length];
            var next = step switch
            {
                'L' => current.Item1,
                'R' => current.Item2,
            };
            yield return (next, i + 1);
            current = map[next];
        }
    }

    return map
        .Where(pair => pair.Key.EndsWith('A'))
        .Select(pair => Walker(pair.Value).GetEnumerator())
        .Select(enumerator =>
        {
            while (true)
            {
                enumerator.MoveNext();
                if (enumerator.Current.Item1.EndsWith('Z'))
                {
                    return (long)enumerator.Current.Item2;
                }

            }
        })
        .LCM()
        .ToString();

}