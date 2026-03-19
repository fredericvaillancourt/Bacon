namespace Bacon.Tests;

public class TestFormattable(string value) : IFormattable
{
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"({value}:{(string.IsNullOrWhiteSpace(format) ? "{null}" : format)})";
    }

    public override string ToString()
    {
        return ToString(null, null);
    }
}