using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

[Generator]
public class ArgumentsGenerator : IIncrementalGenerator
{
    private static readonly Parameter BuildOutput = new(new ParamType("Bacon.Build.IBuildOutput", SupportedParamType.IsNullable), "BuildOutput", null);
    private static readonly SyntaxToken[] DefaultTokensWithBase =
    [
        new(SyntaxTokenType.Base, '\0', null),
        new(SyntaxTokenType.Literal, '\0', " "),
        new(SyntaxTokenType.Args, ' ', "--")
    ];

    private static readonly SyntaxToken[] DefaultTokensWithoutBase =
    [
        new(SyntaxTokenType.Args, ' ', "--")
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "Bacon.Build.SyntaxAttribute",
            static (_, _) => true,
            static (syntaxContext, cancellationToken) =>
            {
                var argumentsClass = syntaxContext.TargetNode as ClassDeclarationSyntax;
                if (argumentsClass == null)
                {
                    return null;
                }

                var argumentsType = syntaxContext.TargetSymbol as INamedTypeSymbol;
                if (argumentsType == null)
                {
                    return null;
                }

                string? syntax = syntaxContext.Attributes.FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value as string;
                if (syntax == null)
                {
                    return null;
                }

                var argumentsActualType = syntaxContext.SemanticModel.Compilation.GetTypeByMetadataName("Bacon.Build.Arguments");

                INamedTypeSymbol? lastBeforeArguments = null;
                INamedTypeSymbol? parent = null;
                INamedTypeSymbol? current = argumentsType;
                while (current != null && !SymbolEqualityComparer.Default.Equals(current, argumentsActualType))
                {
                    lastBeforeArguments = current;
                    current = current.BaseType;
                    parent ??= current;
                }

                if (current == null)
                {
                    return null;
                }

                var parameters = argumentsClass
                    .Members
                    .OfType<PropertyDeclarationSyntax>()
                    .Where(static p => p.Modifiers.Any(static m => m.IsKind(SyntaxKind.PublicKeyword)))
                    .Select(p => Parameter.From(
                        syntaxContext.SemanticModel,
                        p.Type,
                        p.Identifier.ValueText,
                        GetParameterName(p.AttributeLists, syntaxContext.SemanticModel, cancellationToken)))
                    .Where(static p => p != null)
                    .Select<Parameter?, Parameter>(static p => p!)
                    .ToArray();

                return ArgumentsInfo.From(argumentsClass, argumentsType, lastBeforeArguments!, parameters, syntax);
            }).Where(static s => s != null).Select<ArgumentsInfo?, ArgumentsInfo>(static (s, _) => s!).Collect();

        context.RegisterSourceOutput(syntaxProvider, static (productionContext, node) =>
        {
            var dic = node.ToDictionary(static k => k.FullClassName);
            foreach (ArgumentsInfo argumentsInfo in node)
            {
                if (argumentsInfo.ParentFullClassName != null && dic.TryGetValue(argumentsInfo.ParentFullClassName, out var parent))
                {
                    argumentsInfo.Base = parent;
                }
            }

            foreach (var info in node)
            {
                using var writer = new StringWriter();
                using var iw = new IndentedTextWriter(writer, "    ");

                iw.WriteHeader();
                iw.WriteNamespace(info.Namespace);

                if (!info.IsAbstract)
                {
                    GenerateArgumentToolMethods(iw, info);
                }

                iw.WriteLine($"public {(info.IsAbstract ? "abstract " : "")}partial class {info.ClassName}");
                iw.OpenBracket();

                GenerateConstructor(iw, info);
                iw.WriteLine();

                GenerateDoFormat(iw, info);
                iw.WriteLine();

                if (!info.IsAbstract)
                {
                    GenerateImplicitCasts(iw, info);
                }

                GenerateBuilder(iw, info);
                iw.CloseBracket();

                productionContext.AddSource($"{info.ClassName}.g.cs", writer.ToString());
            }
        });
    }

    private static void GenerateBuilder(IndentedTextWriter iw, ArgumentsInfo info)
    {
        string baseClass = !info.IsRoot ? $" : {info.Base?.ClassName}.Builder" : " : Bacon.Build.Arguments.Builder";
        if (!info.IsAbstract)
        {
            baseClass += $", Bacon.Build.IArgumentsBuilder<Builder, {info.ClassName}>";
        }

        iw.WriteLine($"public new {(info.IsAbstract ? "abstract " : "")}partial class Builder{baseClass}");
        iw.OpenBracket();
        iw.WriteLine($"{(info.IsAbstract ? "protected" : "public")} Builder()");
        iw.OpenBracket();
        iw.CloseBracket();
        iw.WriteLine();

        iw.WriteLine($"{(info.IsAbstract ? "protected" : "public")} Builder({info.ClassName} value)");
        if (info.Base != null)
        {
            ++iw.Indent;
            iw.WriteLine(": base(value)");
            --iw.Indent;
        }

        iw.OpenBracket();

        foreach (var parameter in info.Parameters)
        {
            iw.WriteLine($"{parameter.Name} = value.{parameter.Name};");
        }

        iw.CloseBracket();
        iw.WriteLine();

        if (!info.IsAbstract)
        {
            iw.WriteLine($"Builder Bacon.Build.IArgumentsBuilder<Builder, {info.ClassName}>.Clone()");
            iw.OpenBracket();
            iw.WriteLine("var builder = new Builder();");

            foreach (var parameter in info.Parameters)
            {
                iw.WriteLine($"builder.{parameter.Name} = {parameter.Name};");
            }

            iw.WriteLine("return builder;");
            iw.CloseBracket();
        }

        foreach (var parameter in info.Parameters)
        {
            GenerateBuilderParameter(iw, parameter, false);
        }

        foreach (var parameter in info.BaseParameters)
        {
            GenerateBuilderParameter(iw, parameter, true);
        }

        GenerateBuilderParameter(iw, BuildOutput, true);

        if (!info.IsAbstract)
        {
            iw.WriteLine($"public {(info.Base?.IsAbstract == false ? "new " : "")}{info.ClassName} Build()");
            iw.OpenBracket();

            foreach (Parameter parameter in info.Parameters)
            {
                if (parameter.Type.IsNullable || parameter.Type.IsBool)
                {
                    continue;
                }

                iw.WriteLine(parameter.Type.IsValueType ?
                    $"if (!{parameter.Name}.HasValue)" :
                    $"if ({parameter.Name} == null)");

                iw.OpenBracket();
                iw.WriteLine($"throw new System.InvalidOperationException(\"{parameter.Name} cannot be null.\");");
                iw.CloseBracket();
            }

            iw.WriteLine($"return new {info.ClassName}(this);");
            iw.CloseBracket();
        }

        iw.CloseBracket();
    }

    private static void GenerateImplicitCasts(IndentedTextWriter iw, ArgumentsInfo info)
    {
        iw.WriteLine($"public static implicit operator {info.ClassName}(Builder builder)");
        iw.OpenBracket();
        iw.WriteLine("return builder.Build();");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine($"public static implicit operator Builder({info.ClassName} arguments)");
        iw.OpenBracket();
        iw.WriteLine("return new Builder(arguments);");
        iw.CloseBracket();
        iw.WriteLine();
    }

    private static void GenerateConstructor(IndentedTextWriter iw, ArgumentsInfo info)
    {
        iw.WriteLine($"protected {info.ClassName}(Builder builder)");

        ++iw.Indent;
        iw.WriteLine(": base(builder)");
        --iw.Indent;

        iw.OpenBracket();

        foreach (var parameter in info.Parameters)
        {
            string extra = "";
            if (parameter.Type is { IsNullable: false, IsBool: false })
            {
                if (!parameter.Type.IsValueType)
                {
                    extra = "!";
                }
                else if (!parameter.Type.IsArray)
                {
                    extra = "!.Value";
                }
            }

            iw.WriteLine($"{parameter.Name} = builder.{parameter.Name}{extra};");
        }

        iw.CloseBracket();
    }

    private static void GenerateArgumentToolMethods(IndentedTextWriter iw, ArgumentsInfo info)
    {
        string name = info.ToolName != "DotNet" ? info.ToolName : "Bacon.Build.DotNet";
        iw.WriteLine($"public static partial class {info.ToolName}ToolExtensions");
        iw.OpenBracket();
        iw.WriteLine($"public static Bacon.Build.Result {info.ActionName}(this {name} self, Func<{info.ClassName}.Builder, {info.ClassName}> configure)");
        iw.OpenBracket();
        iw.WriteLine($"return self.{info.ActionName}(configure(new {info.ClassName}.Builder()));");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine($"public static Bacon.Build.Result {info.ActionName}(this {name} self, {info.ClassName} arguments)");
        iw.OpenBracket();
        iw.WriteLine("return self.Tool.Execute(arguments.Format(), arguments.BuildOutput);");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine($"public static Bacon.Build.Result[] {info.ActionName}(this {name} self, Func<{info.ClassName}.Builder, System.Collections.Generic.IEnumerable<{info.ClassName}>> configure)");
        iw.OpenBracket();
        iw.WriteLine($"var results = new System.Collections.Generic.List<Bacon.Build.Result>();");
        iw.WriteLine($"foreach (var arguments in configure(new {info.ClassName}.Builder()))");
        iw.OpenBracket();
        iw.WriteLine($"results.Add(self.{info.ActionName}(arguments));");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine("return results.ToArray();");
        iw.CloseBracket();
        iw.CloseBracket();
        iw.WriteLine();
    }

    private static void GenerateDoFormat(IndentedTextWriter iw, ArgumentsInfo info)
    {
        iw.WriteLine("protected override void DoFormat(ref Bacon.Build.ArgumentsBuilder builder)");
        iw.OpenBracket();

        if (!SyntaxParser.TryParse(info.Syntax, out var parsed))
        {
            parsed = info.Base != null ? DefaultTokensWithBase : DefaultTokensWithoutBase;
        }

        foreach (SyntaxToken syntaxToken in parsed)
        {
            switch (syntaxToken.Type)
            {
                case SyntaxTokenType.Literal:
                    iw.WriteLine("builder.AddSpaceIfRequired();");
                    iw.WriteLine($"builder.Append({SymbolDisplay.FormatLiteral(syntaxToken.Value!.Trim(), true)});");
                    break;
                case SyntaxTokenType.Base:
                    iw.WriteLine("base.DoFormat(ref builder);");
                    break;
                case SyntaxTokenType.Args:
                    foreach (var parameter in info.Parameters)
                    {
                        if (parameter.Type.IsBool)
                        {
                            iw.WriteLine($"if ({parameter.Name}{(parameter.Type.IsNullable ? ".HasValue" : "")})");

                            iw.OpenBracket();
                            if (parameter.Type.IsNullable)
                            {
                                iw.WriteLine($"builder.Append($\" {SymbolDisplay.FormatLiteral(syntaxToken.Value!, false)}{parameter.CommandLine}{syntaxToken.Separator}{{{parameter.Name}.Value}}\");");
                            }
                            else
                            {
                                iw.WriteLine($"builder.Append(\" {SymbolDisplay.FormatLiteral(syntaxToken.Value!, false)}{parameter.CommandLine}\");");
                            }
                            iw.CloseBracket();
                        }
                        else
                        {
                            if (parameter.Type.IsNullable)
                            {
                                iw.WriteLine(parameter.Type.IsValueType ?
                                    $"if ({parameter.Name}.HasValue)" :
                                    $"if ({parameter.Name} != null)");
                                iw.OpenBracket();
                            }

                            string appendCommandLine;
                            if (parameter.CommandLine != null)
                            {
                                appendCommandLine = $"builder.Append(\" {SymbolDisplay.FormatLiteral(syntaxToken.Value!, false)}{parameter.CommandLine}{syntaxToken.Separator}\");";
                            }
                            else
                            {
                                appendCommandLine = $"builder.Append(' ');";
                            }

                            string extraCalls = "";
                            if (parameter.Type.IsEnum)
                            {
                                extraCalls = parameter.Type is { IsNullable: true, IsArray: false } ? ".Value.ToValueString()" : ".ToValueString()";
                            }
                            else if (parameter.Type is { IsNullable: true, IsValueType: true })
                            {
                                extraCalls = ".Value";
                            }

                            bool isString = parameter.Type.IsString;

                            if (!parameter.Type.IsArray)
                            {
                                AppendOneParameter(iw, appendCommandLine, parameter.Name, isString, extraCalls);
                            }
                            else
                            {
                                iw.WriteLine($"for (int i = 0; i < {parameter.Name}.Length; ++i)");
                                iw.OpenBracket();
                                AppendOneParameter(iw, appendCommandLine, parameter.Name, isString, $"[i]{extraCalls}");
                                iw.CloseBracket();
                            }

                            if (parameter.Type.IsNullable)
                            {
                                iw.CloseBracket();
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        iw.CloseBracket();

        static void AppendOneParameter(IndentedTextWriter iw, string appendCommandLine, string parameterName, bool isString, string extraCalls)
        {
            iw.WriteLine(appendCommandLine);
            iw.WriteLine($"builder.{(isString ? "Safe" : "")}Append({parameterName}{extraCalls});");
        }
    }

    private static void GenerateBuilderParameter(IndentedTextWriter iw, Parameter parameter, bool isNew)
    {
        string typeAsString = parameter.Type is { IsBool: true, IsNullable: false } ? "bool" : parameter.Type.AsNullable();
        if (!isNew)
        {
            iw.WriteLine($"public {typeAsString} {parameter.Name} {{ get; set; }}");
            iw.WriteLine();
        }

        string prefix = isNew ? "new " : "";

        if (parameter.Type is { IsBool: true, IsNullable: false })
        {
            iw.WriteLine($"public {prefix}Builder Enable{parameter.Name}()");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name} = true;");
            iw.WriteLine("return this;");
            iw.CloseBracket();
            iw.WriteLine();
            iw.WriteLine($"public {prefix}Builder Disable{parameter.Name}()");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name} = false;");
            iw.WriteLine("return this;");
            iw.CloseBracket();
        }
        else
        {
            iw.WriteLine($"public {prefix}Builder Set{parameter.Name}({typeAsString} value)");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name} = value;");
            iw.WriteLine("return this;");
            iw.CloseBracket();
            iw.WriteLine();
            iw.WriteLine($"public {prefix}Builder Reset{parameter.Name}()");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name} = null;");
            iw.WriteLine("return this;");
            iw.CloseBracket();
        }

        iw.WriteLine();
    }

    private static string? GetParameterName(
        SyntaxList<AttributeListSyntax> attributeList,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var expression = attributeList
            .SelectMany(static a => a.Attributes)
            .FirstOrDefault(a => a.Name.ToString() is "Parameter" or "ParameterAttribute")?
            .ArgumentList?
            .Arguments
            .FirstOrDefault()?
            .Expression;

        if (expression == null)
        {
            return null;
        }

        var constant = semanticModel.GetConstantValue(expression, cancellationToken);
        return constant is { HasValue: true, Value: string s } ? s : null;
    }
}