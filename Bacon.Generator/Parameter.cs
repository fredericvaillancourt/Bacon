using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

internal sealed record Parameter(
    ParamType Type,
    string Name,
    string? CommandLine,
    bool IsSecret,
    string? Join)
{
    public static Parameter? From(SemanticModel semanticModel, TypeSyntax type, string name, string? commandLine, bool isSecret, string? join)
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
        if (notNullableType.Name == "IReadOnlyList")
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

        if (notNullableType.TypeKind == TypeKind.Enum)
        {
            t |= SupportedParamType.Enum;
        }
        else
        {
            t |= notNullableType.SpecialType switch
            {
                SpecialType.System_String => SupportedParamType.String,
                SpecialType.System_Boolean => SupportedParamType.Bool,
                SpecialType.System_Byte => SupportedParamType.Numeric,
                SpecialType.System_SByte => SupportedParamType.Numeric,
                SpecialType.System_UInt16 => SupportedParamType.Numeric,
                SpecialType.System_Int16 => SupportedParamType.Numeric,
                SpecialType.System_UInt32 => SupportedParamType.Numeric,
                SpecialType.System_Int32 => SupportedParamType.Numeric,
                SpecialType.System_UInt64 => SupportedParamType.Numeric,
                SpecialType.System_Int64 => SupportedParamType.Numeric,
                SpecialType.System_Single => SupportedParamType.Numeric,
                SpecialType.System_Double => SupportedParamType.Numeric,
                SpecialType.System_Decimal => SupportedParamType.Numeric,
                SpecialType.System_Char => SupportedParamType.Char,
                _ => notNullableType.ToString() switch
                {
                    "System.Half" => SupportedParamType.Numeric,
                    "System.Int128" => SupportedParamType.Numeric,
                    "System.UInt128" => SupportedParamType.Numeric,
                    "System.Numerics.BigInteger" => SupportedParamType.Numeric,
                    _ => 0
                }
            };
        }

        return new Parameter(
            new ParamType(notNullableType.ToString(), t),
            name,
            commandLine,
            isSecret,
            join);
    }
}