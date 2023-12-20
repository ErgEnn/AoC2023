using AoC.Util;

var exampleInput =
    """
    px{a<2006:qkq,m>2090:A,rfg}
    pv{a>1716:R,A}
    lnx{m>1548:A,A}
    rfg{s<537:gd,x>2440:R,A}
    qs{s>3448:A,lnx}
    qkq{x<1416:A,crn}
    crn{x>2662:A,R}
    in{s<1351:px,qqz}
    qqz{s>2770:qs,m<1801:hdj,R}
    gd{a>3333:R,R}
    hdj{m>838:A,pv}
    
    {x=787,m=2655,a=1222,s=2876}
    {x=1679,m=44,a=2067,s=496}
    {x=2036,m=264,a=79,s=2244}
    {x=2461,m=1339,a=466,s=291}
    {x=2127,m=1623,a=2188,s=1013}
    """;

var realInput = File.ReadAllText("input.txt");

Console.WriteLine($"Answer 1: {Solve1(exampleInput)}");
Console.WriteLine($"Answer 2: {Solve2(exampleInput)}");

string Solve1(string input)
{
    var lines = input.Lines();
    var isWorkflow = true;
    var workflows = new Dictionary<string, string[]>();
    Condition? inNode = null;
    long total = 0;
    foreach (var line in lines)
    {
        if (isWorkflow)
        {
            if (line.IsNullOrWhitespace())
            {
                isWorkflow = false;
                inNode = ParseTree(workflows, "in", 0);
                continue;
            }

            var (key, conditions) = line.DeconstructRegex<string, string[]>("(.+){(?:([^,]+),?)+}");
            workflows.Add(key, conditions);
        }
        else
        {
            var partAspects = line.Trim('{', '}').Split(',').Select(aspect => aspect.Split('=')[1].ToInt()).ToArray();
            if(inNode!.Invoke(partAspects))
                total += partAspects.Sum();
        }
    }
    return total.ToString();
}

Condition ParseTree(Dictionary<string,string[]> workflows, string key, int conditionIndex)
{
    if (key == "A" || key == "R")
    {
        return new Condition(-1, false, key == "A" ? 1 : -1, null, null);
    }
    var condition = workflows[key][conditionIndex];
    if (workflows[key].Length-1 == conditionIndex)
    {
        return ParseTree(workflows, condition, 0);
    }
    var colonIndex = condition.IndexOf(':');
    var aspectIndex = "xmas".IndexOf(condition[0]);
    var greaterThan = condition[1] == '>';
    var threshold = condition[2..colonIndex].ToInt();
    var ifTrueKey = condition[(colonIndex + 1)..];
    return new Condition(aspectIndex, greaterThan, threshold, ParseTree(workflows, ifTrueKey, 0),
        ParseTree(workflows, key, conditionIndex + 1));
}

string Solve2(string input)
{
    var lines = input.Lines();
    var workflows = new Dictionary<string, string[]>();
    Condition? inNode = null;
    long total = 0;
    foreach (var line in lines)
    {
        if (line.IsNullOrWhitespace())
        {
            inNode = ParseTree(workflows, "in", 0);
            break;
        }

        var (key, conditions) = line.DeconstructRegex<string, string[]>("(.+){(?:([^,]+),?)+}");
        workflows.Add(key, conditions);
    }

    var acceptedRanges = new List<AspectRanges>();
    WalkTree(new AspectRanges(1, 4000, 1, 4000, 1, 4000, 1, 4000), inNode!, acceptedRanges);

    return acceptedRanges.Sum(ranges => (long)(ranges.maxX-ranges.minX+1)* (ranges.maxM - ranges.minM + 1)* (ranges.maxA - ranges.minA + 1)* (ranges.maxS - ranges.minS + 1)).ToString();
}

void WalkTree(AspectRanges ranges, Condition condition, List<AspectRanges> acceptedRanges)
{
    if (ranges.minX > ranges.maxX
        || ranges.minM > ranges.maxM
        || ranges.minA > ranges.maxA
        || ranges.minS > ranges.maxS)
        return;
    switch (condition.AspectIndex)
    {
        case -1:
            if (condition.Threshold > 0)
            {
                acceptedRanges.Add(ranges);
            }
            break;
        case 0:
            if (condition.GreaterThan) // x > threshold
            {
                WalkTree(ranges with{minX = Math.Max(ranges.minX,condition.Threshold+1)}, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with{maxX = Math.Min(ranges.maxX,condition.Threshold)}, condition.IfFalse, acceptedRanges);
            }
            else // x < threshold
            {
                WalkTree(ranges with {maxX = Math.Min(ranges.maxX, condition.Threshold-1)}, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with {minX = Math.Max(ranges.minX, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            break;
        case 1:
            if (condition.GreaterThan) // m > threshold
            {
                WalkTree(ranges with { minM = Math.Max(ranges.minM, condition.Threshold + 1) }, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with { maxM = Math.Min(ranges.maxM, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            else // m < threshold
            {
                WalkTree(ranges with { maxM = Math.Min(ranges.maxM, condition.Threshold - 1) }, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with { minM = Math.Max(ranges.minM, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            break;
        case 2:
            if (condition.GreaterThan) // a > threshold
            {
                WalkTree(ranges with { minA = Math.Max(ranges.minA, condition.Threshold + 1) }, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with { maxA = Math.Min(ranges.maxA, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            else // a < threshold
            {
                WalkTree(ranges with { maxA = Math.Min(ranges.maxA, condition.Threshold - 1) }, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with { minA = Math.Max(ranges.minA, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            break;
        case 3:
            if (condition.GreaterThan) // s > threshold
            {
                WalkTree(ranges with { minS = Math.Max(ranges.minS, condition.Threshold + 1) }, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with { maxS = Math.Min(ranges.maxS, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            else // s < threshold
            {
                WalkTree(ranges with { maxS = Math.Min(ranges.maxS, condition.Threshold - 1) }, condition.IfTrue, acceptedRanges);
                WalkTree(ranges with { minS = Math.Max(ranges.minS, condition.Threshold) }, condition.IfFalse, acceptedRanges);
            }
            break;
    }
}

record AspectRanges(int minX, int maxX, int minM, int maxM, int minA, int maxA, int minS, int maxS);

class Condition(int aspectIndex, bool greaterThan, int threshold, Condition ifTrue, Condition ifFalse)
{
    public int AspectIndex { get; } = aspectIndex;
    public bool GreaterThan { get; } = greaterThan;
    public int Threshold { get; } = threshold;
    public Condition IfTrue { get; } = ifTrue;
    public Condition IfFalse { get; } = ifFalse;


    public bool Invoke(int[] partAspects)
    {
        if (AspectIndex == -1)
            if (Threshold > 0)
                return true;
            else
                return false;
        else
            if (GreaterThan)
                if (partAspects[AspectIndex] > Threshold)
                    return IfTrue.Invoke(partAspects);
                else
                    return IfFalse.Invoke(partAspects);
            else
                if (partAspects[AspectIndex] < Threshold)
                    return IfTrue.Invoke(partAspects);
                else
                    return IfFalse.Invoke(partAspects);
    }
}