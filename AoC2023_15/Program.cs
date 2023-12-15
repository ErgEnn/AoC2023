using AoC.Util;

var exampleInput =
    """
    rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    return input
        .Split(',')
        .Select(s => s
            .Select(c => (int) c)
            .SelectWithStorage<int,int>((accumulator, number) => (accumulator + number) * 17 % 256))
        .Sum().ToString();
}

string Solve2(string input)
{
    var boxes = new List<(string, int)>[256];
    for (int i = 0; i < 256; i++)
        boxes[i] = new List<(string, int)>();

    var steps = input
        .Split(',')
        .Select(step => step.EndsWith('-') ? (step[..^1], 0) : (step[..^2], step[^1].ToInt()))
        .ToArray();

    foreach ((string key, int val) step in steps)
    {
        var hash = step.key
            .Select(c => (int)c)
            .SelectWithStorage<int, int>((accumulator, number) => (accumulator + number) * 17 % 256);
        if (step.val == 0)
        {
            boxes[hash].RemoveAll(lens => lens.Item1 == step.key);
        }
        else
        {
            int index = boxes[hash].FindIndex(tuple => tuple.Item1 == step.key);
            if (index != -1)
            {
                boxes[hash].RemoveAll(lens => lens.Item1 == step.key);
                boxes[hash].Insert(index,step);
            }
            else
            {
                boxes[hash].Add(step);
            }
        }
    }

    return boxes
        .Select((box, boxIndex) => box
            .Select((lens, lensIndex) => (boxIndex + 1) * (lensIndex + 1) * lens.Item2)
            .Sum())
        .Sum()
        .ToString();
}