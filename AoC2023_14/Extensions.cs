using AoC.Util;

namespace AoC2023_14;

public static class Extensions
{

    public static IEnumerable<string> FallLeft(this IEnumerable<string> rows)
    {

        (int predicateCnt, int fullCount) Counts(IEnumerable<char> chrs, Predicate<char> predicate)
        {
            int predicateCnt = 0, fullCount = 0;
            foreach (var chr in chrs)
            {
                fullCount++;
                if (predicate(chr))
                    predicateCnt++;
            }

            return (predicateCnt, fullCount);
        }
        return rows.Select(column =>
        {
            string Take(IEnumerable<char> chrs, int skip = 0)
            {
                var cnts = Counts(chrs.Skip(skip).TakeWhile(c => c != '#'), c => c == 'O');
                var hashTagCnt = chrs.Skip(skip + cnts.fullCount).TakeWhile(c => c == '#').Count();
                if (cnts.fullCount == 0 && hashTagCnt == 0)
                    return String.Empty;
                var nextPart = Take(chrs, skip + cnts.fullCount + hashTagCnt);
                if (nextPart == String.Empty)
                    return "O".Repeat(cnts.predicateCnt) + ".".Repeat(cnts.fullCount - cnts.predicateCnt) +
                           "#".Repeat(hashTagCnt);
                return "O".Repeat(cnts.predicateCnt) + ".".Repeat(cnts.fullCount - cnts.predicateCnt) +
                       "#".Repeat(hashTagCnt) + nextPart;
            }

            return Take(column);
        });
    }
}