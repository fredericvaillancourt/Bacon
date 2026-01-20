using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

internal record ArgumentsInfo(
    string? Namespace,
    string ClassName,
    string FullClassName,
    string? ParentFullClassName,
    bool IsAbstract,
    string Syntax,
    string ToolName,
    string ActionName,
    EquatableArray<Parameter> Parameters)
{
    public IEnumerable<Parameter> BaseParameters
    {
        get
        {
            var parent = Base;
            while (parent != null)
            {
                for (int i = 0; i < parent.Parameters.Length; ++i)
                {
                    yield return parent.Parameters[i];
                }

                parent = parent.Base;
            }
        }
    }

    public ArgumentsInfo? Base { get; set; }

    public bool IsRoot => Base == null;

    public static ArgumentsInfo From(
        ClassDeclarationSyntax argumentsClassSyntax,
        INamedTypeSymbol argumentsClassSymbol,
        INamedTypeSymbol topClassSymbol,
        EquatableArray<Parameter> parameters,
        string syntax)
    {
        string className = argumentsClassSyntax.Identifier.ValueText;
        string toolName = RemoveArguments(topClassSymbol.Name);
        string actionName = GetActionName(className, toolName);

        return new ArgumentsInfo(
            argumentsClassSymbol.ContainingNamespace.ToStringOrNull(),
            className,
            argumentsClassSymbol.ToDisplayString(),
            !SymbolEqualityComparer.Default.Equals(argumentsClassSymbol, topClassSymbol) ?
                argumentsClassSymbol.BaseType?.ToDisplayString() :
                null,
            argumentsClassSymbol.IsAbstract,
            syntax,
            toolName,
            actionName,
            parameters);
    }

    private static string RemoveArguments(string s)
    {
        return s.EndsWith("Arguments") ? s[..^9] : s;
    }

    private static string GetActionName(string s, string toolName)
    {
        string actionName = s.StartsWith(toolName) && s.EndsWith("Arguments") ? s[(toolName.Length)..^9] : s;
        return actionName.Length > 0 ? actionName : "Execute";
    }
}