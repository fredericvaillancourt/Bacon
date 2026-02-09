using System.Collections;

namespace Bacon.Build;

public sealed class ArgumentDictionaryBuilder(string separator = ";") : IEnumerable<KeyValuePair<string, string>>
{
    private readonly string _separator = separator;
    private readonly List<KeyValuePair<string, string>> _items = new();

    public ArgumentDictionaryBuilder Add(string key, string value)
    {
        _items.Add(new KeyValuePair<string, string>(key, value));
        return this;
    }

    public ArgumentDictionaryBuilder Add(params KeyValuePair<string, string>[] values)
    {
        _items.AddRange(values);
        return this;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_items).GetEnumerator();
    }

    public override string ToString()
    {
        if (_items.Count == 0)
        {
            return string.Empty;
        }

        int size = (_items.Count - 1) * _separator.Length + _items.Count; // N - 1 separator + N equal
        foreach (var kv in _items)
        {
            size += kv.Key.Length + kv.Value.Length;
        }

        return string.Create(size, this, static (span, builder) =>
        {
            using var e = builder._items.GetEnumerator();
            e.MoveNext();

            e.Current.Key.CopyTo(span);
            int index = e.Current.Key.Length;
            span[index++] = '=';
            e.Current.Value.CopyTo(span[index..]);
            index += e.Current.Value.Length;

            while (e.MoveNext())
            {
                builder._separator.CopyTo(span[index..]);
                index += builder._separator.Length;

                e.Current.Key.CopyTo(span[index..]);
                index += e.Current.Key.Length;
                span[index++] = '=';
                e.Current.Value.CopyTo(span[index..]);
                index += e.Current.Value.Length;
            }
        });
    }

    public static implicit operator string(ArgumentDictionaryBuilder builder)
    {
        return builder.ToString();
    }

    public static implicit operator string[](ArgumentDictionaryBuilder builder)
    {
        string[] results = new string[builder._items.Count];
        int i = 0;

        foreach (var kv in builder._items)
        {
            results[i++] = $"{kv.Key}={kv.Value}";
        }

        return results;
    }
}