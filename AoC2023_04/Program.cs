using AoC.Util;

var exampleInput =
    """
    Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
    Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
    Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
    Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
    Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
    Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");

string Solve1(string input)
{
    var lines = input.Lines();

    var totalPnts = lines.Select(line =>
    {
        var (left, right) = line.Deconstruct<string, string>(" | ");
        var (_, cardNosStr) = left.Deconstruct<string, string>(":");
        var winningNos = right.Split(' ').Where(s => !s.IsNullOrWhitespace());
        var cardNos = cardNosStr.Split(' ').Where(s => !s.IsNullOrWhitespace());
        var winningCnt = winningNos.Select(winninNo => cardNos.Contains(winninNo)).Count(b => b);
        if (winningCnt > 0)
            return Math.Pow(2, winningCnt - 1);
        return 0;
    }).Sum();
    return totalPnts.ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var cardCnt = new Dictionary<int, int>();
    var totalPnts = lines.Select((line, index) =>
    {
        cardCnt.Increment(index);
        for (int _ = 0; _ < cardCnt[index]; _++)
        {
            var (left, right) = line.Deconstruct<string, string>(" | ");
            var (_, cardNosStr) = left.Deconstruct<string, string>(":");
            var winningNos = right.Split(' ').Where(s => !s.IsNullOrWhitespace());
            var cardNos = cardNosStr.Split(' ').Where(s => !s.IsNullOrWhitespace());
            var winningCnt = winningNos.Select(winninNo => cardNos.Contains(winninNo)).Count(b => b);
            for (int i = winningCnt; i > 0; i--)
            {
                cardCnt.Increment(index+i);
            }
        }

        return 0;

    }).Count();
    return cardCnt.Values.Sum(i => i).ToString();
}