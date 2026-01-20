using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

[Generator]
public class ToolGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "Bacon.Build.ToolAttribute",
            static (_, _) => true,
            static (syntaxContext, _) =>
            {
                if (syntaxContext.TargetNode is not ClassDeclarationSyntax toolSyntax)
                {
                    return null;
                }

                var args = syntaxContext.Attributes.FirstOrDefault()?.ConstructorArguments;
                if (args == null)
                {
                    return null;
                }

                string? commandName = args.Value[0].Value as string;
                var location = (ToolLocation)args.Value[1].Value!;
                if (string.IsNullOrWhiteSpace(commandName))
                {
                    return null;
                }

                var buildOutputType = (args.Value[2].Value as INamedTypeSymbol)?.ToString();
                return syntaxContext.TargetSymbol is INamedTypeSymbol toolSymbol ?
                    ToolInfo.From(commandName!, location, buildOutputType, toolSyntax, toolSymbol) :
                    null;
            }).Where(static s => s != null).Select<ToolInfo?, ToolInfo>(static (s, _) => s!).WithTrackingName("ToolAttribute");

        context.RegisterSourceOutput(syntaxProvider, static (productionContext, info) =>
        {
            using var writer = new StringWriter();
            using var iw = new IndentedTextWriter(writer, "    ");

            iw.WriteHeader();
            iw.WriteNamespace(info.Namespace);

            iw.WriteLine($"public sealed partial class {info.ToolName}(Bacon.Build.Context context)");
            iw.OpenBracket();
            iw.WriteLine("private static readonly object Key = new();");
            iw.WriteLine();
            iw.WriteLine("public Bacon.Build.ITool<string, Bacon.Build.Result> Tool");
            iw.OpenBracket();
            iw.WriteLine($"get => context.GetOrAdd(Key, static ctx => ctx.{GetToolLocationCommand(info.Location)}({SymbolDisplay.FormatLiteral(info.CommandName, true)}{(info.BuildOutputType != null ? $", new {info.BuildOutputType}(ctx.BuildOutput)" : "")}));");
            iw.WriteLine("set => context.Set(Key, value);");
            iw.CloseBracket();
            iw.CloseBracket();
            iw.WriteLine();

            iw.WriteLine($"public static class {info.ToolName}Extensions");
            iw.OpenBracket();
            iw.WriteLine($"extension(Bacon.Build.Context self)");
            iw.OpenBracket();
            iw.WriteLine($"public {info.ToolName} {info.ToolName} => new(self);");
            iw.CloseBracket();
            iw.CloseBracket();
            iw.WriteLine();

            productionContext.AddSource($"{info.ToolName}.g.cs", writer.ToString());
        });
    }

    private static string GetToolLocationCommand(ToolLocation location)
    {
        return location switch
        {
            ToolLocation.Path => "SearchForCommand",
            ToolLocation.Tool => "SearchForTool",
            ToolLocation.Nuget => throw new NotSupportedException(),
            ToolLocation.Solution => throw new NotSupportedException(),
            ToolLocation.Project => throw new NotSupportedException(),
            ToolLocation.Absolute => throw new NotSupportedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };
    }
}

//WARNING: Must match Build
internal enum ToolLocation
{
    Path,
    Tool,
    Nuget,
    Solution,
    Project,
    Absolute
}