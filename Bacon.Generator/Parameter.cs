using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

internal record Parameter(
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
            
        if (typeToCheck.IsKind(SyntaxKind.ArrayType))
        {
            typeToCheck = ((ArrayTypeSyntax)typeToCheck).ElementType;
            t |= SupportedParamType.IsArray;

            if (typeToCheck.IsKind(SyntaxKind.NullableType))
            {
                return null;
            }
        }

        //TODO: Should probably check with the semantic type to support Bool ... also all numerics
        if (typeToCheck is PredefinedTypeSyntax p)
        {
            if (p.Keyword.IsKind(SyntaxKind.StringKeyword))
            {
                t |= SupportedParamType.String;
            }
            else if (p.Keyword.IsKind(SyntaxKind.BoolKeyword))
            {
                t |= SupportedParamType.Bool;
            }
            else if (p.Keyword.IsKind(SyntaxKind.IntKeyword))
            {
                t |= SupportedParamType.Numeric;
            }
        }
        
        var typeToCheckSymbol = semanticModel.GetTypeInfo(typeToCheck);
        var notNullableType = typeToCheckSymbol.Type ?? throw new InvalidOperationException("Missing type");
        if (notNullableType.TypeKind == TypeKind.Enum)
        {
            t |= SupportedParamType.Enum;
        }

        return new Parameter(
            new ParamType(notNullableType.ToString(), t),
            name,
            commandLine);
    }
}