using AoC.Util;

var exampleInput =
    """
    32T3K 765
    T55J5 684
    KK677 28
    KTJJT 220
    QQQJA 483
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve2(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var result = lines
        .Select(line => line.Deconstruct<string, int>())
        .OrderBy(tuple => tuple.s1, new Comparator())
        .Select((tuple, i) => (i + 1) * tuple.s2)
        .Sum();
    return result.ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var result = lines
        .Select(line => line.Deconstruct<string, int>())
        .OrderBy(tuple => tuple.s1, new Comparator2())
        .Select((tuple, i) => (i + 1) * tuple.s2)
        .Sum();
    return result.ToString();
}

class Comparator : IComparer<string>
{
    private string order = "AKQJT98765432";

    public int Compare(string? x, string? y)
    {
        var l1 = GetLabel(x);
        var l2 = GetLabel(y);
        if (l1 == l2)
        {
            for (int i = 0; i < 5; i++)
            {
                if (order.IndexOf(x[i]) == order.IndexOf(y[i]))
                    continue;
                return order.IndexOf(y[i]) - order.IndexOf(x[i]);
            }

            return 0;
        }

        return l2 - l1;
    }

    private int GetLabel(string x)
    {
        var distinct = x.Distinct().ToArray();
        if (distinct.Length == 1)
            return 1;
        if (distinct.Length == 2)
            if (x.Count(c => distinct[0] == c) is 1 or 4)
                return 2;
            else
                return 3;
        if (distinct.Length == 3)
            if (distinct.Any(c => x.Count(c1 => c1 == c) == 3))
                return 4;
            else
                return 5;
        if (distinct.Length == 4)
            return 6;
        return 7;
    }
}

class Comparator2 : IComparer<string>
{
    private string order = "AKQT98765432J";

    public int Compare(string? x, string? y)
    {
        var l1 = GetLabelWithJokers(x);
        var l2 = GetLabelWithJokers(y);
        if (l1 == l2)
        {
            for (int i = 0; i < 5; i++)
            {
                if (order.IndexOf(x[i]) == order.IndexOf(y[i]))
                    continue;
                return order.IndexOf(y[i]) - order.IndexOf(x[i]);
            }

            return 0;
        }

        return l2 - l1;
    }

    private int GetLabelWithJokers(string x)
    {
        var distinct = x.Distinct().ToArray();
        if (x == "JJJJJ")
            return 1;
        if (distinct.Any(c => c == 'J'))
        {
            return distinct.Where(c => c != 'J')
                .Select(c => GetLabel(x.Replace('J',c))).MinBy(i => i);
        }

        return GetLabel(x);
    }

    private int GetLabel(string x)
    {
        var distinct = x.Distinct().ToArray();
        if (distinct.Length == 1)
            return 1;
        if (distinct.Length == 2)
            if (x.Count(c => distinct[0] == c) is 1 or 4)
                return 2;
            else
                return 3;
        if (distinct.Length == 3)
            if (distinct.Any(c => x.Count(c1 => c1 == c) == 3))
                return 4;
            else
                return 5;
        if (distinct.Length == 4)
            return 6;
        return 7;
    }
}