using AoC.Util;

var exampleInput =
    """
    seeds: 79 14 55 13
    
    seed-to-soil map:
    50 98 2
    52 50 48
    
    soil-to-fertilizer map:
    0 15 37
    37 52 2
    39 0 15
    
    fertilizer-to-water map:
    49 53 8
    0 11 42
    42 0 7
    57 7 4
    
    water-to-light map:
    88 18 7
    18 25 70
    
    light-to-temperature map:
    45 77 23
    81 45 19
    68 64 13
    
    temperature-to-humidity map:
    0 69 1
    1 0 69
    
    humidity-to-location map:
    60 56 37
    56 93 4
    
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(realInput)}");
Console.WriteLine($"Answer 2: {Solve2(realInput)}");

string Solve1(string input)
{
    var lines = input.Lines().ToList();
    var lineEnumerator = lines.GetEnumerator();
    lineEnumerator.MoveNext();
    var (_, seedsCsv) = lineEnumerator.Current.Deconstruct<string, string>(":");
    var seeds = seedsCsv.Split(' ').Where(s => !s.IsNullOrWhitespace()).Select(s => s.ToLong()).ToArray();
    var preMappedVals = seeds;
    var mappedVals = new long[seeds.Length];
    lineEnumerator.MoveNext(); // empty line

    for (int mapper = 0; mapper < 7; mapper++)
    {
        lineEnumerator.MoveNext();
        Console.WriteLine(lineEnumerator.Current);
        var isMapped = new bool[seeds.Length];
        while (lineEnumerator.MoveNext())
        {
            if (lineEnumerator.Current.IsNullOrWhitespace()) // end of mapping directive
            {
                foreach (var (i, _) in isMapped.Indexed().Where(tuple => !tuple.value))
                {
                    mappedVals[i] = preMappedVals[i];
                }
                break;
            }

            var (toStart, fromStart, length) = lineEnumerator.Current.Deconstruct<long, long, long>();
            foreach ((int i, long preMappedVal) in preMappedVals.Indexed())
            {
                if (preMappedVal.IsBetween(fromStart, fromStart + length))
                {
                    mappedVals[i] = toStart + (preMappedVal - fromStart);
                    isMapped[i] = true;
                    Console.WriteLine($"Mapped {preMappedVals[i]} -> {mappedVals[i]}; {fromStart} {toStart} {length}");
                }
            }
        }
        preMappedVals = mappedVals;
        mappedVals = new long[seeds.Length];
    }
    
    return preMappedVals.Min().ToString();
}

((long overlapStart, long overlapLen)?, (long unmappedStart, long unmappedLen)[] unmapped) FindOverlaps(long rangeStart,
    long rangeEnd, long mapperStart, long mapperEnd)
{
    (long start, long len) ToStartLen(long start, long end) => (start, end - start + 1);

    if (mapperStart < rangeStart)
    {
        if (mapperEnd > rangeEnd)
        {
            var mappedRange = ToStartLen(rangeStart, rangeEnd);
            return (mappedRange, Array.Empty<(long unmappedStart, long unmappedEnd)>());
        }
        else if (mapperEnd == rangeEnd)
        {
            var mappedRange = ToStartLen(rangeStart, rangeEnd);
            return (mappedRange, Array.Empty<(long unmappedStart, long unmappedEnd)>());
        }
        else if (mapperEnd < rangeEnd)
        {
            if (rangeStart < mapperEnd)
            {
                var mappedRange = ToStartLen(rangeStart, mapperEnd);
                var unmappedRange = ToStartLen(mapperEnd + 1, rangeEnd);
                return (mappedRange, new[] { unmappedRange });
            }
            else
            {
                var unmappedRange = ToStartLen(rangeStart, rangeEnd);
                return (null, new[] { unmappedRange });
            }
        }
    }
    else if (mapperStart == rangeStart)
    {
        if (mapperEnd > rangeEnd)
        {
            var mappedRange = ToStartLen(rangeStart, rangeEnd);
            return (mappedRange, Array.Empty<(long unmappedStart, long unmappedEnd)>());
        }
        else if (mapperEnd == rangeEnd)
        {
            var mappedRange = ToStartLen(rangeStart, rangeEnd);
            return (mappedRange, Array.Empty<(long unmappedStart, long unmappedEnd)>());
        }
        else if (mapperEnd < rangeEnd)
        {
            var mappedRange = ToStartLen(rangeStart, mapperEnd);
            var unmappedRange = ToStartLen(mapperEnd + 1, rangeEnd);
            return (mappedRange, new[] { unmappedRange });
        }
    }
    else if (mapperStart > rangeStart)
    {
        if (mapperEnd > rangeEnd)
        {
            if (mapperStart > rangeEnd)
            {
                var unmappedRange = ToStartLen(rangeStart, rangeEnd);
                return (null, new[] { unmappedRange });
            }
            else
            {
                var mappedRange = ToStartLen(mapperStart, rangeEnd);
                var unmappedRange = ToStartLen(rangeStart, mapperStart - 1);
                return (mappedRange, new[] { unmappedRange });
            }
        }
        else if (mapperEnd == rangeEnd)
        {
            var mappedRange = ToStartLen(mapperStart, rangeEnd);
            var unmappedRange = ToStartLen(rangeStart, mapperStart - 1);
            return (mappedRange, new[] { unmappedRange });
        }
        else if (mapperEnd < rangeEnd)
        {
            var mappedRange = ToStartLen(mapperStart, mapperEnd);
            var unmappedRange1 = ToStartLen(rangeStart, mapperStart - 1);
            var unmappedRange2 = ToStartLen(mapperEnd + 1, rangeEnd);
            return (mappedRange, new[] { unmappedRange1, unmappedRange2 });
        }
    }

    throw new NotImplementedException();
}

string ToStartEnd((long, long) range) => $"{range.Item1},{range.Item1 + range.Item2 - 1}";

string Solve2(string input)
{
    var lines = input.Lines().ToList();
    var lineEnumerator = lines.GetEnumerator();
    lineEnumerator.MoveNext();
    var (_, seedsCsv) = lineEnumerator.Current.Deconstruct<string, string>(":");
    var seedRanges = seedsCsv.Split(' ').Where(s => !s.IsNullOrWhitespace()).Select(s => s.ToLong()).Pairwise(overlapping:false).ToArray();
    
    lineEnumerator.MoveNext(); // empty line

    (long rangeStart, long rangeLen)[] Map((long rangeStart, long rangeLen)[] ranges)
    {
        lineEnumerator.MoveNext();
        Console.WriteLine(lineEnumerator.Current);
        var unmappedRanges = new List<(long, long)>(ranges);
        var mappedRanges = new List<(long, long)>();
        while (lineEnumerator.MoveNext())
        {

            if (lineEnumerator.Current.IsNullOrWhitespace()) // end of mapping directive
            {
                break;
            }

            var (toStart, mapperStart, mapperLength) = lineEnumerator.Current.Deconstruct<long, long, long>();
            var mapperDelta = toStart - mapperStart;
            var unmappeds = unmappedRanges.ToArray();
            unmappedRanges.Clear();
            foreach ((long rangeStart, long rangeLen) in unmappeds)
            {
                var mapperEnd = mapperStart + mapperLength - 1;
                var rangeEnd = rangeStart + rangeLen - 1;

                var overlaps = FindOverlaps(rangeStart, rangeEnd, mapperStart, mapperEnd);

                if (overlaps.Item1 != null)
                {
                    var range = (overlaps.Item1.Value.overlapStart + mapperDelta, overlaps.Item1.Value.overlapLen);
                    mappedRanges.Add(range);
                    Console.WriteLine($"  Mapped {overlaps.Item1} <{ToStartEnd(overlaps.Item1.Value)}> {mapperDelta:+0;-#}");
                }

                unmappedRanges.AddRange(overlaps.unmapped);
            }
        }

        foreach (var unmappedRange in unmappedRanges)
        {
            Console.WriteLine($"  Unmapped {unmappedRange} <{ToStartEnd(unmappedRange)}>");
        }
        return mappedRanges.Concat(unmappedRanges).ToArray();
    }

    var ranges = seedRanges;
    for (int _ = 0; _ < 7; _++)
    {
        ranges = Map(ranges);
    }

    return ranges.Select(tuple => tuple.i1).Min().ToString();
}