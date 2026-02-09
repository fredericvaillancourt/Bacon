namespace Bacon.Build;

internal static class EnumerableExtensions
{
    public static bool TryGetFirstOrDefault<TSource>(this IEnumerable<TSource> source, out TSource? value)
    {
        if (source is IReadOnlyList<TSource> list)
        {
            if (list.Count > 0)
            {
                value = list[0];
                return true;
            }
        }
        else
        {
            using var e = source.GetEnumerator();
            if (e.MoveNext())
            {
                value = e.Current;
                return true;
            }
        }

        value = default;
        return false;
    }
}