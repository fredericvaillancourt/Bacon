using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Bacon.Generator;

internal static class SyntaxParser
{
    // args defaults to --args value
    // value has quote if not alphanumeric
    // specifying " will force them
    // --args=value
    // -args:value
    // /args value
    // or
    // --args="value"
    // -args:"value"
    // /args "value"
    // or
    // "--args=value"
    // "-args:value"
    // "/args value"
    private static readonly Regex ArgumentsRegex = new("^(?'quote0'\")?(?'prefix'--|-|/)?args(?:(?'separator'[ :=])(?'quote1'\")?value)?(?'quote2'\")?$");

    public static bool TryParse(string value, [NotNullWhen(true)] out IEnumerable<SyntaxToken>? parsed)
    {
        var result = new List<SyntaxToken>();

        int startIndex = 0;
        bool inside = false;
        int i = 0;
        while (i < value.Length)
        {
            char c = value[i];
            if (c == '{')
            {
                // Escape { with {
                if (i + 1 < value.Length && value[i + 1] == '{')
                {
                    ++i;
                    continue;
                }

                if (inside)
                {
                    goto Error;
                }

                if (startIndex < i - 1)
                {
                    result.Add(new SyntaxToken(SyntaxTokenType.Literal, SyntaxQuoteStyle.Automatic, '\0', value[startIndex..i]));
                }

                inside = true;
                startIndex = i + 1;
            }
            else if (c == '}')
            {
                // Escape } with }
                if (i + 1 < value.Length && value[i + 1] == '}')
                {
                    ++i;
                    continue;
                }

                if (!inside)
                {
                    goto Error;
                }

                inside = false;
                SyntaxToken token;
                int length = i - startIndex;
                if (length == 4 && string.Compare(value, startIndex, "base", 0, 4, StringComparison.Ordinal) == 0)
                {
                    token = new SyntaxToken(SyntaxTokenType.Base, SyntaxQuoteStyle.Automatic, '\0', null);
                }
                else
                {
                    var match = ArgumentsRegex.Match(value, startIndex, length);
                    if (!match.Success)
                    {
                        goto Error;
                    }

                    var prefixGroup = match.Groups["prefix"];
                    var separatorGroup = match.Groups["separator"];
                    var quoteStyle = SyntaxQuoteStyle.Automatic;
                    bool quote0 = match.Groups["quote0"].Success;
                    bool quote1 = match.Groups["quote1"].Success;
                    bool quote2 = match.Groups["quote2"].Success;
                    if (quote2)
                    {
                        if (quote0 && !quote1)
                        {
                            quoteStyle = SyntaxQuoteStyle.Whole;
                        }
                        else if (!quote0 && quote1)
                        {
                            quoteStyle = SyntaxQuoteStyle.Value;
                        }
                    }

                    token = new SyntaxToken(
                        SyntaxTokenType.Args,
                        quoteStyle,
                        separatorGroup.Success ? separatorGroup.Value[0] : ' ',
                        prefixGroup.Success ? prefixGroup.Value : "--");
                }

                startIndex = i + 1;
                result.Add(token);
            }

            ++i;
        }

        if (inside)
        {
            goto Error;
        }

        if (startIndex < i - 1)
        {
            result.Add(new SyntaxToken(SyntaxTokenType.Literal, SyntaxQuoteStyle.Automatic, '\0', value[startIndex..(i - 1)]));
        }

        parsed = result;
        return true;
        Error:
        parsed = null;
        return false;
    }
}