using AoC.Util;

var exampleInput =
    """
    Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
    Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
    Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
    Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
    Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");



string Solve1(string input)
{
    var lines = input.Lines();


    var thresholds = new Dictionary<string, int>()
    {
        {"red", 12},
        {"green", 13},
        {"blue", 14}
    };

    var possibleSum = 0;
    foreach (var line in lines)
    {
        var (gameId, game) = line.Deconstruct<string, string>(":");
        var subsets = game.Split(';');
        foreach (var subset in subsets)
        {
            var cubes = subset.Split(',');
            foreach (var cube in cubes)
            {
                var (amount, color) = cube.Trim().Deconstruct<int, string>();
                if (amount > thresholds[color])
                {
                    goto nextLine;
                }
            }
        }

        var (_, id) = gameId.Deconstruct<string, int>();
        possibleSum += id;
        nextLine: ;
    }

    return possibleSum.ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    var possibleSum = 0;
    foreach (var line in lines)
    {
        var colors = new Dictionary<string, int>()
        {
            {"red", 0},
            {"green", 0},
            {"blue", 0},
        };

        var (gameId, game) = line.Deconstruct<string, string>(":");
        var subsets = game.Split(';');
        foreach (var subset in subsets)
        {
            var cubes = subset.Split(',');
            foreach (var cube in cubes)
            {
                var (amount, color) = cube.Trim().Deconstruct<int, string>();
                colors[color] = Math.Max(amount, colors[color]);
            }
        }

        var (_, id) = gameId.Deconstruct<string, int>();
        possibleSum += colors["red"] * colors["green"] * colors["blue"];

    }

    return possibleSum.ToString();
}
