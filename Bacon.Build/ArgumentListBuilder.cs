using System.Collections;

namespace Bacon.Build;

public sealed class ArgumentListBuilder(string separator = ";") : IEnumerable<string>
{
    private readonly List<string> _items = new();

    public ArgumentListBuilder Add(params string[] values)
    {
        _items.AddRange(values);
        return this;
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_items).GetEnumerator();
    }

    public override string ToString()
    {
        return string.Join(separator, _items);
    }

    public static implicit operator string(ArgumentListBuilder builder)
    {
        return builder.ToString();
    }
}