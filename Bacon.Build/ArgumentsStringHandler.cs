using System.Buffers;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bacon.Build;

[InterpolatedStringHandler]
public struct ArgumentsStringHandler(int literalLength, int formattedCount)
{
    private static readonly SearchValues<char> DoNotNeedQuotes = SearchValues.Create("0123456789:-./ABCDEFGHIJKLMNOPQRSTUVWXYZ\\_abcdefghijklmnopqrstuvwxyz");
    private readonly StringBuilder _redacted = new(literalLength + formattedCount * 16);
    private StringBuilder? _secret;

    public ArgumentsStringHandler()
        : this(256, 0)
    {
    }

    public void AppendLiteral(string value)
    {
        _redacted.Append(value);
        _secret?.Append(value);
    }

    public void AppendLiteral(char value)
    {
        _redacted.Append(value);
        _secret?.Append(value);
    }

    public void AddSpaceIfRequired()
    {
        if (_redacted.Length > 0 && _redacted[^1] != ' ')
        {
            AppendLiteral(' ');
        }
    }

    public void AppendFormatted(string? value)
    {
        AppendRedacted(value, false);
    }

    public void AppendFormatted(string? value, string format)
    {
        if (value == null)
        {
            return;
        }

        (bool secret, bool hasQuotes, string? actualFormat) = ParseFormat(format);

        if (actualFormat != null)
        {
            throw new ArgumentException("Invalid format string", nameof(format));
        }

        if (!secret)
        {
            AppendRedacted(value, hasQuotes);
            return;
        }

        AppendSecret(value, hasQuotes);
    }

    public void AppendFormatted<T>(T? value)
    {
        AppendRedacted(Format(value, null), false);
    }

    public void AppendFormatted<T>(T? value, string format)
    {
        if (value == null)
        {
            return;
        }

        (bool secret, bool hasQuotes, string? actualFormat) = ParseFormat(format);

        if (!secret)
        {
            AppendRedacted(Format(value, actualFormat), hasQuotes);
            return;
        }

        AppendSecret(Format(value, actualFormat), hasQuotes);
    }

    public string GetRedactedString() => _redacted.ToString();
    public string? GetSecretString() => _secret?.ToString();

    public override string ToString() => GetRedactedString();

    private void AppendRedacted(string? value, bool hasQuotes)
    {
        if (value == null)
        {
            return;
        }

        AppendEscaped(value, hasQuotes, _redacted, _secret);
    }

    private void AppendSecret(string? value, bool hasQuotes)
    {
        if (value != null)
        {
            if (_secret == null)
            {
                _secret = new(_redacted.Capacity);
                _secret.Append(_redacted);
            }

            AppendEscaped(value, hasQuotes, _secret);
        }

        _redacted.Append("[redacted]");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? Format<T>(T value, string? format) =>
        value switch
        {
            IFormattable f => f.ToString(format, CultureInfo.InvariantCulture),
            _ => value!.ToString()
        };

    private static void AppendEscaped(string value, bool hasQuotes, StringBuilder always, StringBuilder? sometime = null)
    {
        var span = value.AsSpan();

        if (span.IsEmpty)
        {
            if (!hasQuotes)
            {
                always.Append("\"\"");
                sometime?.Append("\"\"");
            }

            return;
        }

        bool needQuotes = !hasQuotes && span.ContainsAnyExcept(DoNotNeedQuotes);
        if (!needQuotes)
        {
            always.Append(value);
            sometime?.Append(value);
            return;
        }

        always.Append('"');
        sometime?.Append('"');

        int quotes = span.IndexOf('"');

        while (quotes >= 0)
        {
            var toAppend = span[..quotes];

            var backslash = Backslash(toAppend);
            always.Append(toAppend);
            always.Append(backslash);
            always.Append('\\');
            always.Append(span[quotes]);

            if (sometime != null)
            {
                sometime.Append(toAppend);
                sometime.Append(backslash);
                sometime.Append('\\');
                sometime.Append(span[quotes]);
            }

            span = span[(quotes + 1)..];
            quotes = span.IndexOf('"');
        }

        var lastBackslash = Backslash(span);
        always.Append(span);
        always.Append(lastBackslash);
        always.Append('"');

        if (sometime != null)
        {
            sometime.Append(span);
            sometime.Append(lastBackslash);
            sometime.Append('"');
        }
    }

    private static (bool Secret, bool HasQuotes, string? Format) ParseFormat(string? format)
    {
        if (string.IsNullOrEmpty(format))
        {
            return default;
        }

        switch (format)
        {
            case "?":
                return (true, false, null);
            case "\"":
                return (false, true, null);
            case "?\"":
            case "\"?":
                return (true, true, null);
        }

        bool secret = false;
        bool hasQuotes = false;
        string actualFormat = format;
        if (format[0] == '?')
        {
            secret = true;
            if (format.Length > 1 && format[1] == '"')
            {
                hasQuotes = true;
                actualFormat = format[2..];
            }
            else
            {
                actualFormat = format[1..];
            }
        }
        else if (format[0] == '"')
        {
            hasQuotes = true;
            if (format.Length > 1 && format[1] == '?')
            {
                secret = true;
                actualFormat = format[2..];
            }
            else
            {
                actualFormat = format[1..];
            }
        }

        return (secret, hasQuotes, actualFormat);
    }

    private static ReadOnlySpan<char> Backslash(ReadOnlySpan<char> value)
    {
        int i = value.Length;
        while (i > 0)
        {
            int next = i - 1;
            if (value[next] != '\\')
            {
                break;
            }

            i = next;
        }

        return value[i..];
    }
}