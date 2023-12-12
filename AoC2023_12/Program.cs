using System.Text;
using AoC.Util;

var exampleInput =
    """
    ???.### 1,1,3
    .??..??...?##. 1,1,3
    ?#?#?#?#?#?#?#? 1,3,1,6
    ????.#...#... 4,1,1
    ????.######..#####. 1,6,5
    ?###???????? 3,2,1
    """;


var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    return lines.Select((line, lineNo) =>
    {
        var (springs, counts) = line.Deconstruct<string, int[]>();
        //Console.WriteLine(line);
        var unknowns = springs.Indexed().Where(tuple => tuple.value == '?').Select(tuple => tuple.i).ToArray();
        var possibleSolutions = 0;
        for (int i = 0; i < Math.Pow(2, unknowns.Length); i++)
        {
            var cnts = new List<int>();
            var activeCnt = 0;
            foreach ((int index, char spring) in springs.Indexed())
            {
                var varSpring = spring;
                if (spring == '?')
                {
                    var unknownNo = unknowns.IndexOf(index);
                    if ((i & (1 << unknownNo)) == (1 << unknownNo))
                        varSpring = '#';
                    else
                        varSpring = '.';
                }

                //Console.Write(varSpring);
                if (varSpring == '.')
                {
                    if (activeCnt > 0)
                    {
                        cnts.Add(activeCnt);
                        activeCnt = 0;
                    }
                    continue;
                }

                if (varSpring == '#')
                {
                    activeCnt++;
                    continue;
                }
            }
            if (activeCnt > 0)
            {
                cnts.Add(activeCnt);
            }
            if(cnts.Count != counts.Length || !cnts.SequenceEqual(counts))
                goto nextLoop;
            //Console.Write(" - OK");
            possibleSolutions++;

            nextLoop:
            //Console.WriteLine();
            continue;
        }
        //Console.WriteLine($"-----------{lineNo+1} ({possibleSolutions})-------------");
        return possibleSolutions;
    }).Sum().ToString();
}

string Solve2(string input)
{
    var lines = input.Lines();
    return lines.Select((line, lineNo) =>
    {
        var multiplier = 5;
        var (springs, countsStr) = line.Deconstruct<string, string>();
        springs = string.Join("?", Enumerable.Repeat(springs, multiplier));
        countsStr = string.Join(",", Enumerable.Repeat(countsStr, multiplier));
        var counts = countsStr.Split(',').ToInts().ToArray();

        var cache = new Dictionary<(int, int), long>();
        IEnumerable<int> FindPossibleSpanStarts(int start, int size, int countIndex)
        {
            for (int spanStart = start; spanStart < springs.Length-size+1; spanStart++)
            {
                if(springs.AsSpan(spanStart,size).Contains('.'))
                    continue;
                if (spanStart + size < springs.Length && springs[spanStart+size]=='#')
                    if (!springs.AsSpan(spanStart, size).Contains('?'))
                        yield break;
                    else
                        continue;
                if (spanStart - 1 >= 0 && springs[spanStart-1] == '#')
                    continue;
                if(springs.AsSpan(start..spanStart).Contains('#'))
                    yield break;
                if(countIndex+1 == counts.Length && springs.AsSpan((spanStart+size)..).Contains('#'))
                    continue;
                yield return spanStart;
                if (!springs.AsSpan(spanStart,size).Contains('?'))
                    yield break;
            }
        }

        long Recursive(int min, int countIndex, int[] arr)
        {
            if (cache.ContainsKey((min, countIndex)))
                return cache[(min, countIndex)];

            if (countIndex == counts.Length)
            {
                if (false)
                {
                    foreach (var i in arr)
                    {
                        Console.CursorLeft = i;
                        Console.Write(i);
                    }
                    Console.WriteLine();
                    var sb = new StringBuilder();
                    for (int i = 0; i < springs.Length; i++)
                    {
                        if (arr.Contains(i))
                        {
                            var sze = counts[arr.IndexOf(i)];
                            sb.Append(string.Join("", Enumerable.Repeat("#", sze)));
                            i += sze - 1;
                        }
                        else
                        {
                            sb.Append(springs[i]);
                        }
                    }

                    Console.WriteLine(sb.ToString());
                }
                return 1;
            }
            

            var size = counts[countIndex];
            long sum = 0;
            foreach (var start in FindPossibleSpanStarts(min, size, countIndex))
            {
                sum += Recursive(start + size + 1, countIndex + 1, arr.Concat(start).ToArray());
            }
            cache.Add((min,countIndex), sum);
            return sum;
        }

        return Recursive(0, 0, new int[0]);
    }).Sum().ToString();
}