using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

internal sealed record Parameter(
    ParamType Type,
    string Name,
    string? CommandLine)
{
    public static Parameter? From(SemanticModel semanticModel, TypeSyntax type, string name, string? commandLine)
    {
        SupportedParamType t = 0;

        TypeSyntax typeToCheck = type;
        if (typeToCheck.IsKind(SyntaxKind.NullableType))
        {
            typeToCheck = ((NullableTypeSyntax)type).ElementType;
            t |= SupportedParamType.IsNullable;
        }

        var typeToCheckSymbol = semanticModel.GetTypeInfo(typeToCheck);
        var notNullableType = typeToCheckSymbol.Type ?? throw new InvalidOperationException("Missing type");
        if (notNullableType.TypeKind == TypeKind.Enum)
        {
            t |= SupportedParamType.Enum;
        }
        else if (notNullableType.Name == "IReadOnlyList")
        {
            notNullableType = ((INamedTypeSymbol)notNullableType).TypeArguments[0];
            t |= SupportedParamType.IsList;
        }
        else if (notNullableType.Name == "IReadOnlyDictionary")
        {
            var namedTypeSymbol = (INamedTypeSymbol)notNullableType;
            if (namedTypeSymbol.TypeArguments[0].SpecialType != SpecialType.System_String)
            {
                return null;
            }

            notNullableType = namedTypeSymbol.TypeArguments[1];
            t |= SupportedParamType.IsDictionary;
        }

        t |= notNullableType.SpecialType switch
        {
            SpecialType.System_String => SupportedParamType.String,
            SpecialType.System_Boolean => SupportedParamType.Bool,
            SpecialType.System_Int32 => SupportedParamType.Numeric,
            _ => 0
        };

        return new Parameter(
            new ParamType(notNullableType.ToString(), t),
            name,
            commandLine);
    }
}