using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

[Generator]
public sealed class EnumGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "Bacon.Build.EnumParameterAttribute",
            static (_, _) => true,
            static (syntaxContext, _) =>
            {
                //TODO: Validate it ends with Arguments?
                if (syntaxContext.TargetNode is not EnumDeclarationSyntax enumSyntax)
                {
                    return null;
                }

                string enumName = enumSyntax.Identifier.ValueText;
                var model = syntaxContext.SemanticModel;
                var members = new List<EnumMemberInfo>();
                foreach (EnumMemberDeclarationSyntax enumMemberSyntax in enumSyntax.Members)
                {
                    string name = enumMemberSyntax.Identifier.ValueText;
                    var attribute = enumMemberSyntax
                        .AttributeLists
                        .SelectMany(static a => a.Attributes)
                        .FirstOrDefault(static a => ((IdentifierNameSyntax)a.Name).Identifier.ValueText == "EnumName")
                        ?.ArgumentList
                        ?.Arguments
                        .FirstOrDefault();

                    string? value = null;
                    if (attribute != null)
                    {
                        value = model.GetConstantValue(attribute.Expression).Value as string;
                    }

                    members.Add(new EnumMemberInfo(name, value ?? name));
                }

                return new EnumInfo(enumName, syntaxContext.TargetSymbol.ContainingNamespace.ToStringOrNull(), members.ToArray());
            }).Where(static s => s != null).Select<EnumInfo?, EnumInfo>(static (s, _) => s!).WithTrackingName("EnumParameterAttribute");

        context.RegisterSourceOutput(syntaxProvider, static (productionContext, info) =>
        {
            using var writer = new StringWriter();
            using var iw = new IndentedTextWriter(writer, "    ");

            iw.WriteHeader();
            iw.WriteNamespace(info.Namespace);

            iw.WriteLine($"public static class {info.Name}Extensions");
            iw.OpenBracket();
            iw.WriteLine($"extension({info.Name} value)");
            iw.OpenBracket();
            iw.WriteLine("public string ToValueString() =>");
            ++iw.Indent;
            iw.WriteLine("value switch");
            iw.OpenBracket();

            foreach (EnumMemberInfo member in info.Members)
            {
                iw.WriteLine($"{info.Name}.{member.Name} => {SymbolDisplay.FormatLiteral(member.Value, true)},");
            }

            iw.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)");
            --iw.Indent;
            iw.WriteLine("};");
            --iw.Indent;
            iw.CloseBracket();
            iw.CloseBracket();

            productionContext.AddSource($"{info.Name}.g.cs", writer.ToString());
        });
    }
}