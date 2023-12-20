using AoC.Util;

var exampleInput =
    """
    2413432311323
    3215453535623
    3255245654254
    3446585845452
    4546657867536
    1438598798454
    4457876987766
    3637877979653
    4654967986887
    4564679986453
    1224686865563
    2546548887735
    4322674655533
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    return Pathfind(lines, 1, 3);
}

string Solve2(string input)
{
    var lines = input.Lines();
    return Pathfind(lines, 4, 10);
}

string Pathfind(string[] lines, int minStraightLineLen, int maxStraightLineLen)
{
    var goal = lines.GetLastCharIndex();
    var (maxX, maxY) = lines.GetLastCharIndex();
    var queue = new PriorityQueue<((int x, int y) coord, (int dx, int dy) cameFrom), int>();
    queue.Enqueue(((0, 0), (0, 0)), 0);
    var visited = new HashSet<((int x, int y) coord, (int dx, int dy) cameFrom)>();
    var costs = new Dictionary<((int x, int y), (int dx, int dy)), int>();
    costs.Add(((0, 0), (0, 0)), 0);

    while (queue.TryDequeue(out var node, out var heatLoss))
    {
        visited.Add(node);
        foreach (var delta in node.coord.NeighborDeltas(maxX, maxY))
        {
            if (delta == node.cameFrom)
                continue;
            if (delta == (0, 0).Subtract(node.cameFrom))
                continue;

            for (int i = minStraightLineLen; i <= maxStraightLineLen; i++)
            {
                var multipliedDelta = delta.Multiply(i);
                (int x, int y) next = node.coord.Add(multipliedDelta);
                if (next.AnyLessThan((0, 0)) || next.AnyGreaterThan((maxX, maxY)))
                    continue;
                if (visited.Contains((next, delta)))
                    continue;
                var newCost = heatLoss;
                for (int j = 1; j <= i; j++)
                {
                    newCost += lines[node.coord.y + delta.y * j][node.coord.x + delta.x * j].ToInt();
                }

                if (costs.TryGetValue((next, delta), out var currentCost))
                {
                    if (currentCost > newCost)
                    {
                        queue.Enqueue((next, delta), newCost);
                        costs[(next, delta)] = newCost;
                    }
                }
                else
                {
                    queue.Enqueue((next, delta), newCost);
                    costs.Add((next, delta), newCost);
                }

            }
        }
    }

    return costs.Where(cost => cost.Key.Item1 == goal).MinBy(pair => pair.Value).Value.ToString();
}