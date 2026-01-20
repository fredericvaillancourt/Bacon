using System.Text;

namespace Bacon.Build;

public class ArgumentListBuilder(string separator = ";")
{
    private readonly StringBuilder _sb = new();

    public ArgumentListBuilder Add(params string[] values)
    {
        if (values.Length > 0)
        {
            if (_sb.Length > 0)
            {
                _sb.Append(separator);
            }

            _sb.Append(values[0]);

            for (int i = 1; i < values.Length; ++i)
            {
                _sb.Append(separator);
                _sb.Append(values[i]);
            }
        }

        return this;
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    public static implicit operator string(ArgumentListBuilder builder)
    {
        return builder._sb.ToString();
    }
}