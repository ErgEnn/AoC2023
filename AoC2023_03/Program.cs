using AoC.Util;

var exampleInput =
    """
    467..114..
    ...*......
    ..35...633
    ......#...
    617*......
    .....+.58.
    ..592.....
    ......755.
    ...$.*....
    .664.598..
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var sum = 0;

    foreach ((string? prev, string current, string? next) in lines.AdjacentLines())
    {
        int? numberStartIndex = null;
        for (int i = 0; i <= current.Length; i++)
        {
            if (i<current.Length && current[i].IsDigit())
            {
                if (numberStartIndex == null)
                    numberStartIndex = i;
            }
            else
            {
                if (numberStartIndex != null)
                {
                    var start = numberStartIndex.Value - 1;
                    var end = i + 1;
                    var range = start.ClampMin(0)..end.ClampMax(current.Length);
                    var hasSymbol =
                        (prev?.AsSpan(range).ContainsAnyExcept("0123456789.") ?? false)
                        || current.AsSpan(range).ContainsAnyExcept("0123456789.")
                        || (next?.AsSpan(range).ContainsAnyExcept("0123456789.") ?? false);
                    if (hasSymbol)
                    {
                        sum += current.ClampingSpan(numberStartIndex.Value, i - numberStartIndex.Value).ToInt();
                    }

                    numberStartIndex = null;
                }
            }
        }
    }
    
    return sum.ToString();
}


string Solve2(string input)
{
    var lines = input.Lines();
    
    var gears = new Dictionary<(int, int), List<int>>();
    foreach ((int lineNo, (string? prev, string current, string? next)) in lines.AdjacentLines().Indexed())
    {
        int? numberStartIndex = null;
        for (int i = 0; i <= current.Length; i++)
        {
            if (i < current.Length && current[i].IsDigit())
            {
                if (numberStartIndex == null)
                    numberStartIndex = i;
            }
            else
            {
                if (numberStartIndex != null)
                {
                    var startIndex = numberStartIndex.Value - 1;
                    var endIndex = i + 1;
                    var value = current.ClampingSpan(numberStartIndex.Value, i - numberStartIndex.Value).ToInt();
                    for (int j = startIndex.ClampMin(0); j < endIndex.ClampMax(current.Length); j++)
                    {
                        if (prev != null && prev[j] == '*')
                            gears.CreateOrAppend((j,lineNo-1), value);
                        if (current[j] == '*')
                            gears.CreateOrAppend((j,lineNo), value);
                        if (next != null && next[j] == '*')
                            gears.CreateOrAppend((j,lineNo+1), value);
                    }

                    numberStartIndex = null;
                }
            }
        }
    }

    return gears.Where(pair => pair.Value.Count == 2).Select(pair => pair.Value.Multiply()).Sum().ToString();
}

