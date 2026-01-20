using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Bacon.Generator;

internal static class SyntaxParser
{
    private static readonly Regex ArgumentsRegex = new("^(?'prefix'--|-|/)?args(?:(?'separator'[ :=])value)?$");

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
                    result.Add(new SyntaxToken(SyntaxTokenType.Literal, '\0', value[startIndex..i]));
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
                    token = new SyntaxToken(SyntaxTokenType.Base, '\0', null);
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

                    token = new SyntaxToken(
                        SyntaxTokenType.Args,
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
            result.Add(new SyntaxToken(SyntaxTokenType.Literal, '\0', value[startIndex..(i - 1)]));
        }

        parsed = result;
        return true;
        Error:
        parsed = null;
        return false;
    }
}