using System.Text;
using AoC.Util;


var exampleInput1 =
    """
    1abc2
    pqr3stu8vwx
    a1b2c3d4e5f
    treb7uchet
    """;
var exampleInput2 =
    """
    two1nine
    eightwothree
    abcone2threexyz
    xtwone3four
    4nineeightseven2
    zoneight234
    7pqrstsixteen
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");

string Solve1(string input)
{
    var lines = input.Lines();

    var sum = lines.Select(line =>
    {
        var enumerator = line.AsEnumerable().GetEnumerator();
        int number1 = 0;
        while (enumerator.MoveNext())
        {
            if (Char.IsDigit(enumerator.Current))
            {
                number1 = int.Parse(enumerator.Current.ToString());
                break;
            }
        }

        enumerator = line.AsEnumerable().Reverse().GetEnumerator();
        int number2 = 0;
        while (enumerator.MoveNext())
        {
            if (Char.IsDigit(enumerator.Current))
            {
                number2 = int.Parse(enumerator.Current.ToString());
                break;
            }
        }

        return number1 * 10 + number2;
    }).Sum();

    return sum.ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();

    var numberWords = new Dictionary<string, int>()
    {
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
    };

    var sum = lines.Select(line =>
    {
        var enumerator = line.AsEnumerable().GetEnumerator();
        int number1 = 0;
        var sb1 = new StringBuilder();
        while (enumerator.MoveNext())
        {
            if (Char.IsDigit(enumerator.Current))
            {
                number1 = int.Parse(enumerator.Current.ToString());
                break;
            }

            sb1.Append(enumerator.Current);
            foreach (var (word, val) in numberWords)
            {
                if (sb1.ToString().EndsWith(word))
                {
                    number1 = val;
                    goto endLoop;
                }
            }

        }
        endLoop:
        enumerator = line.AsEnumerable().Reverse().GetEnumerator();
        int number2 = 0;
        sb1.Clear();
        while (enumerator.MoveNext())
        {
            if (Char.IsDigit(enumerator.Current))
            {
                number2 = int.Parse(enumerator.Current.ToString());
                break;
            }

            sb1.Insert(0, enumerator.Current);
            foreach (var (word, val) in numberWords)
            {
                if (sb1.ToString().StartsWith(word))
                {
                    number2 = val;
                    goto endLoop2;
                }
            }
        }

        endLoop2:
        return number1 * 10 + number2;
    }).Sum();

    return sum.ToString();
}
