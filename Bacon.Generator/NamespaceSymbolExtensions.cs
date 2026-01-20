using Microsoft.CodeAnalysis;

namespace Bacon.Generator;

internal static class NamespaceSymbolExtensions
{
    public static string? ToStringOrNull(this INamespaceSymbol namespaceSymbol)
    {
        return namespaceSymbol.IsGlobalNamespace ? null : namespaceSymbol.ToDisplayString();
    }
}