using Bacon.Build;

namespace Bacon.Tests;

internal static class ArgumentsExtensions
{
    public static string FormatToString(this Arguments arguments)
    {
        var handler = new ArgumentsStringHandler();
        arguments.AppendToStringHandler(ref handler);
        return handler.GetSecretString() ?? handler.GetRedactedString();
    }
}