namespace Bacon.Build;

internal ref struct CommandLineParser(ReadOnlySpan<string> arguments)
{
    private readonly ReadOnlySpan<string> _arguments = arguments;
    private int _index = 0;

    public string? Name { get; private set; }
    public string? Value { get; private set; }

    public bool MoveNext(bool consumeAsToggle = false)
    {
        if (_index >= _arguments.Length)
        {
            if (Name != null && Value == null && !consumeAsToggle)
            {
                throw new InvalidOperationException("Missing value");
            }

            Name = null;
            Value = null;
            return false;
        }

        string arg = _arguments[_index++];

        if (arg.StartsWith("--"))
        {
            if (Name != null && Value == null && !consumeAsToggle)
            {
                throw new InvalidOperationException("Missing value");
            }

            Name = arg[2..];
            Value = null;
            return true;
        }

        if (Value != null || consumeAsToggle)
        {
            Name = null;
        }

        Value = arg;
        return true;
    }
}