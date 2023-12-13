using AoC.Util;

var exampleInput =
    """
    #.##..##.
    ..#.##.#.
    ##......#
    ##......#
    ..#.##.#.
    ..##..##.
    #.#.##.#.
    
    #...##..#
    #....#..#
    ..##..###
    #####.##.
    #####.##.
    ..##..###
    #....#..#
    
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    return GetEmptyLineDeliminatedBlocks(input.Lines())
        .Select(block =>
        {
            foreach ((int i,(string i1, string i2))  in block.Pairwise().Indexed())
            {
                if (i1 == i2)
                {
                    for (int j = 1; i-j >= 0 && i+1+j < block.Length; j++)
                    {
                        if (block[i - j] != block[i + 1 + j])
                            goto falseMirror;
                    }

                    return (i + 1)*100;
                    falseMirror:
                    continue;
                }
            }

            var transposed = Transpose(block).ToArray();
            foreach ((int k,(string j1, string j2)) in transposed.Pairwise().Indexed())
            {
                if (j1 == j2)
                {
                    for (int j = 1; k-j >= 0 && k+1+j < transposed.Length; j++)
                    {
                        if (transposed[k - j] != transposed[k + 1 + j])
                            goto falseMirror2;
                    }

                    return k + 1;
                    falseMirror2:
                    continue;
                }
            }

            throw new Exception();
        }).Sum().ToString();
}

string Solve2(string input)
{
    return GetEmptyLineDeliminatedBlocks(input.Lines())
        .Select(block =>
        {
            foreach ((int i, (string i1, string i2)) in block.Pairwise().Indexed())
            {
                var difSum = StrDiff(i1, i2);
                if (difSum <= 1)
                {
                    for (int j = 1; i - j >= 0 && i + 1 + j < block.Length; j++)
                    {
                        difSum += StrDiff(block[i - j], block[i + 1 + j]);
                    }
                    if(difSum==1)
                        return (i + 1) * 100;
                }
            }

            var transposed = Transpose(block).ToArray();
            foreach ((int k, (string j1, string j2)) in transposed.Pairwise().Indexed())
            {
                var difSum = StrDiff(j1, j2);
                if (difSum <= 1)
                {
                    for (int j = 1; k - j >= 0 && k + 1 + j < transposed.Length; j++)
                    {
                        difSum += StrDiff(transposed[k - j], transposed[k + 1 + j]);
                    }
                    if (difSum == 1)
                        return k + 1;
                }
            }

            throw new Exception();
        }).Sum().ToString();
}

int StrDiff(string s1, string s2)
{
    var cnt = 0;
    for (int i = 0; i < s1.Length; i++)
    {
        if (s1[i] != s2[i])
            cnt++;
    }

    return cnt;
}

IEnumerable<string[]> GetEmptyLineDeliminatedBlocks(string[] arr)
{
    var spanStart = 0;
    for (int spanEnd = 0; spanEnd < arr.Length; spanEnd++)
    {
        if (!arr[spanEnd].IsNullOrWhitespace())
            continue;
        yield return arr[spanStart..spanEnd];
        spanStart = spanEnd+1;
    }
}

IEnumerable<string> Transpose(string[] arr)
{
    for (int i = 0; i < arr[0].Length; i++)
    {
        yield return arr.Select(line => line[i]).Join("");
    }
}