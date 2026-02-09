namespace Bacon.Build;

public static class DictionaryExtensions
{
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> self, IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
        foreach (var kv in values)
        {
            self.Add(kv.Key, kv.Value);
        }
    }
}