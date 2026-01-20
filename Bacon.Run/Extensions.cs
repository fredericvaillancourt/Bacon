namespace Bacon.Run;

internal static class Extensions
{
    // Only works if source is in ascending numerical order
    public static IEnumerable<Range> ToRanges(this IEnumerable<int> source)
    {
        using var enumerator = source.GetEnumerator();
        if (enumerator.MoveNext())
        {
            int from = enumerator.Current;
            int to = from;
            while (enumerator.MoveNext())
            {
                if (to + 1 == enumerator.Current)
                {
                    to = enumerator.Current;
                    continue;
                }

                yield return new Range(from, to);
                from = to = enumerator.Current;
            }

            yield return new Range(from, to);
        }
    }
}