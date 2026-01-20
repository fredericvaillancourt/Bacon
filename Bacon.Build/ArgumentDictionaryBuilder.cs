namespace Bacon.Build;

public class ArgumentDictionaryBuilder(string separator = ";")
{
    private readonly List<string> _items = new();

    public ArgumentDictionaryBuilder Add(string key, string value)
    {
        _items.Add($"{key}={value}");
        return this;
    }

    public ArgumentDictionaryBuilder Add(params KeyValuePair<string, string>[] values)
    {
        _items.EnsureCapacity(_items.Count + values.Length);

        foreach (var kv in values)
        {
            _items.Add($"{kv.Key}={kv.Value}");
        }

        return this;
    }

    public override string ToString()
    {
        return string.Join(separator, _items);
    }

    public static implicit operator string(ArgumentDictionaryBuilder builder)
    {
        return builder.ToString();
    }

    public static implicit operator string[](ArgumentDictionaryBuilder builder)
    {
        return builder._items.ToArray();
    }
}