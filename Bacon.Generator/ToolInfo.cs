using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

internal record ToolInfo(
    string CommandName,
    ToolLocation Location,
    string? BuildOutputType,
    string? Namespace,
    string ToolName)
{
    public static ToolInfo From(string commandName, ToolLocation location, string? buildOutputType, ClassDeclarationSyntax argumentsClassSyntax, INamedTypeSymbol argumentsClassSymbol)
    {
        string argumentClassName = argumentsClassSyntax.Identifier.ValueText;
        return new ToolInfo(
            commandName,
            location,
            buildOutputType,
            argumentsClassSymbol.ContainingNamespace.ToStringOrNull(),
            RemoveArguments(argumentClassName));
    }

    private static string RemoveArguments(string s)
    {
        return s.EndsWith("Arguments") ? s[..^9] : s;
    }
}