using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bacon.Generator;

[Generator]
public sealed class ArgumentsGenerator : IIncrementalGenerator
{
    private static readonly Parameter BuildOutput = new(new ParamType("Bacon.Build.IBuildOutput", SupportedParamType.IsNullable), "BuildOutput", null, false, null);
    private static readonly SyntaxToken[] DefaultTokensWithBase =
    [
        new(SyntaxTokenType.Base, SyntaxQuoteStyle.Automatic, '\0', null),
        new(SyntaxTokenType.Literal, SyntaxQuoteStyle.Automatic, '\0', " "),
        new(SyntaxTokenType.Args, SyntaxQuoteStyle.Automatic, ' ', "--")
    ];

    private static readonly SyntaxToken[] DefaultTokensWithoutBase =
    [
        new(SyntaxTokenType.Args, SyntaxQuoteStyle.Automatic, ' ', "--")
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context){
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
                    .Select(p =>
                    {
                        var arguments = GetParameterArguments(p.AttributeLists, syntaxContext.SemanticModel, cancellationToken);
                        return Parameter.From(
                                syntaxContext.SemanticModel,
                                p.Type,
                                p.Identifier.ValueText,
                                arguments.CommandLine,
                                arguments.IsSecret,
                                arguments.Join);
                    })
                    .Where(static p => p != null)
                    .Select<Parameter?, Parameter>(static p => p!)
                    .ToArray();

                return ArgumentsPreInfo.From(argumentsClass, argumentsType, lastBeforeArguments!, parameters, syntax);
            }).NotNull().Collect();

        var transformed = syntaxProvider.SelectMany<ImmutableArray<ArgumentsPreInfo>, ArgumentsInfo>(static (node, ct) =>
        {
            var dic = node.ToDictionary(static k => k.FullClassName, static v => new ArgumentsPreInfoGrouping(v));
            foreach (var grouping in dic.Values)
            {
                var argumentsInfo = grouping.ArgumentsPreInfo;
                if (argumentsInfo.ParentFullClassName != null &&
                    dic.TryGetValue(argumentsInfo.ParentFullClassName, out var parent))
                {
                    grouping.Base = parent;
                }
            }

            return dic.Values.Select(static m => ArgumentsInfo.From(m));
        });

        context.RegisterSourceOutput(transformed, static (productionContext, info) =>
        {
            using var writer = new StringWriter();
            using var iw = new IndentedTextWriter(writer, "    ");

            iw.WriteHeader();
            iw.WriteNamespace(info.ClassInfo.Namespace);

            if (!info.ClassInfo.IsAbstract)
            {
                GenerateArgumentToolMethods(iw, info);
            }

            iw.WriteLine($"public {(info.ClassInfo.IsAbstract ? "abstract " : "")}partial class {info.ClassInfo.ClassName}");
            iw.OpenBracket();

            GenerateConstructor(iw, info);
            iw.WriteLine();

            GenerateFormat(iw, info);
            iw.WriteLine();

            if (!info.ClassInfo.IsAbstract)
            {
                GenerateImplicitCasts(iw, info);
            }

            GenerateBuilder(iw, info);
            iw.CloseBracket();

            productionContext.AddSource($"{info.ClassInfo.ClassName}.g.cs", writer.ToString());
        });
    }

    private static void GenerateBuilder(IndentedTextWriter iw, ArgumentsInfo info)
    {
        string baseClass = info.ParentClassInfo != null ? $" : {info.ParentClassInfo.ClassName}.Builder" : " : Bacon.Build.Arguments.Builder";
        if (!info.ClassInfo.IsAbstract)
        {
            baseClass += $", Bacon.Build.IArgumentsBuilder<Builder, {info.ClassInfo.ClassName}>";
        }

        iw.WriteLine($"public new {(info.ClassInfo.IsAbstract ? "abstract " : "")}partial class Builder{baseClass}");
        iw.OpenBracket();
        iw.WriteLine($"{(info.ClassInfo.IsAbstract ? "protected" : "public")} Builder()");
        iw.OpenBracket();
        foreach (var parameter in info.ClassInfo.Parameters)
        {
            if (parameter.Type.IsCollection)
            {
                iw.WriteLine($"{parameter.Name} = new();");
            }
        }

        iw.CloseBracket();
        iw.WriteLine();

        iw.WriteLine($"{(info.ClassInfo.IsAbstract ? "protected" : "public")} Builder({info.ClassInfo.ClassName} value)");
        if (info.ParentClassInfo != null)
        {
            ++iw.Indent;
            iw.WriteLine(": base(value)");
            --iw.Indent;
        }

        iw.OpenBracket();

        foreach (var parameter in info.ClassInfo.Parameters)
        {
            string extra = "";
            if (parameter.Type.IsList)
            {
                extra = parameter.Type.IsNullable ?
                    "?.ToList() ?? new()" :
                    ".ToList()";
            }
            else if (parameter.Type.IsDictionary)
            {
                extra = parameter.Type.IsNullable ?
                    "?.ToDictionary() ?? new()" :
                    ".ToDictionary()";
            }

            iw.WriteLine($"{parameter.Name} = value.{parameter.Name}{extra};");
        }

        iw.CloseBracket();
        iw.WriteLine();

        if (!info.ClassInfo.IsAbstract)
        {
            iw.WriteLine($"Builder Bacon.Build.IArgumentsBuilder<Builder, {info.ClassInfo.ClassName}>.Clone()");
            iw.OpenBracket();
            iw.WriteLine("var builder = new Builder();");

            foreach (var parameter in info.ClassInfo.Parameters)
            {
                if (parameter.Type.IsList)
                {
                    iw.WriteLine($"builder.{parameter.Name}.AddRange({parameter.Name});");
                }
                else if (parameter.Type.IsDictionary)
                {
                    iw.WriteLine($"Bacon.Build.DictionaryExtensions.AddRange(builder.{parameter.Name}, {parameter.Name});");
                }
                else
                {
                    iw.WriteLine($"builder.{parameter.Name} = {parameter.Name};");
                }
            }

            iw.WriteLine("return builder;");
            iw.CloseBracket();
        }

        foreach (var parameter in info.ClassInfo.Parameters)
        {
            GenerateBuilderParameter(iw, parameter, false);
        }

        foreach (var parameter in info.AllParentParameters)
        {
            GenerateBuilderParameter(iw, parameter, true);
        }

        GenerateBuilderParameter(iw, BuildOutput, true);

        if (!info.ClassInfo.IsAbstract)
        {
            iw.WriteLine($"public {(info.ParentClassInfo?.IsAbstract == false ? "new " : "")}{info.ClassInfo.ClassName} Build()");
            iw.OpenBracket();

            foreach (Parameter parameter in info.ClassInfo.Parameters)
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

            iw.WriteLine($"return new {info.ClassInfo.ClassName}(this);");
            iw.CloseBracket();
        }

        iw.CloseBracket();
    }

    private static void GenerateImplicitCasts(IndentedTextWriter iw, ArgumentsInfo info)
    {
        iw.WriteLine($"public static implicit operator {info.ClassInfo.ClassName}(Builder builder)");
        iw.OpenBracket();
        iw.WriteLine("return builder.Build();");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine($"public static implicit operator Builder({info.ClassInfo.ClassName} arguments)");
        iw.OpenBracket();
        iw.WriteLine("return new Builder(arguments);");
        iw.CloseBracket();
        iw.WriteLine();
    }

    private static void GenerateConstructor(IndentedTextWriter iw, ArgumentsInfo info)
    {
        iw.WriteLine($"protected {info.ClassInfo.ClassName}(Builder builder)");

        ++iw.Indent;
        iw.WriteLine(": base(builder)");
        --iw.Indent;

        iw.OpenBracket();

        foreach (var parameter in info.ClassInfo.Parameters)
        {
            string extra = "";
            if (parameter.Type.IsList)
            {
                extra = parameter.Type.IsNullable ? $".Count > 0 ? builder.{parameter.Name}.ToArray() : null" : ".ToArray()";
            }
            else if (parameter.Type.IsDictionary)
            {
                extra = parameter.Type.IsNullable ? $".Count > 0 ? builder.{parameter.Name}.ToDictionary() : null" : ".ToDictionary()";
            }
            else if (parameter.Type is { IsNullable: false, IsBool: false })
            {
                extra = parameter.Type.IsValueType ? "!.Value" : "!";
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
        iw.WriteLine($"public static Bacon.Build.Result {info.ActionName}(this {name} self, Func<{info.ClassInfo.ClassName}.Builder, {info.ClassInfo.ClassName}> configure)");
        iw.OpenBracket();
        iw.WriteLine($"return self.{info.ActionName}(configure(new {info.ClassInfo.ClassName}.Builder()));");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine($"public static Bacon.Build.Result {info.ActionName}(this {name} self, {info.ClassInfo.ClassName} arguments)");
        iw.OpenBracket();
        iw.WriteLine("var handler = new Bacon.Build.ArgumentsStringHandler();");
        iw.WriteLine("arguments.AppendToStringHandler(ref handler);");
        iw.WriteLine("return self.Tool.Execute(ref handler, arguments.BuildOutput);");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine($"public static Bacon.Build.Result[] {info.ActionName}(this {name} self, Func<{info.ClassInfo.ClassName}.Builder, System.Collections.Generic.IEnumerable<{info.ClassInfo.ClassName}>> configure)");
        iw.OpenBracket();
        iw.WriteLine($"var results = new System.Collections.Generic.List<Bacon.Build.Result>();");
        iw.WriteLine($"foreach (var arguments in configure(new {info.ClassInfo.ClassName}.Builder()))");
        iw.OpenBracket();
        iw.WriteLine($"results.Add(self.{info.ActionName}(arguments));");
        iw.CloseBracket();
        iw.WriteLine();
        iw.WriteLine("return results.ToArray();");
        iw.CloseBracket();

        if (!info.HasRequiredParameters)
        {
            iw.WriteLine();
            iw.WriteLine($"public static Bacon.Build.Result {info.ActionName}(this {name} self)");
            iw.OpenBracket();
            iw.WriteLine($"return self.{info.ActionName}(new {info.ClassInfo.ClassName}.Builder());");
            iw.CloseBracket();
        }

        iw.CloseBracket();
        iw.WriteLine();
    }

    private static void GenerateFormat(IndentedTextWriter iw, ArgumentsInfo info)
    {
        iw.WriteLine("public override void AppendToStringHandler(ref Bacon.Build.ArgumentsStringHandler arguments)");
        iw.OpenBracket();

        if (!SyntaxParser.TryParse(info.Syntax, out var parsed))
        {
            parsed = info.ParentClassInfo != null ? DefaultTokensWithBase : DefaultTokensWithoutBase;
        }

        foreach (SyntaxToken syntaxToken in parsed)
        {
            switch (syntaxToken.Type)
            {
                case SyntaxTokenType.Literal:
                    iw.WriteLine("arguments.AddSpaceIfRequired();");
                    iw.WriteLine($"arguments.AppendLiteral({SymbolDisplay.FormatLiteral(syntaxToken.Value!.Trim(), true)});");
                    break;
                case SyntaxTokenType.Base:
                    iw.WriteLine("base.AppendToStringHandler(ref arguments);");
                    break;
                case SyntaxTokenType.Args:
                    foreach (var parameter in info.ClassInfo.Parameters)
                    {
                        if (parameter.Type.IsBool)
                        {
                            iw.WriteLine($"if ({parameter.Name}{(parameter.Type.IsNullable ? ".HasValue" : "")})");

                            iw.OpenBracket();
                            if (parameter.Type.IsNullable)
                            {
                                string sep0 = syntaxToken.QuoteStyle == SyntaxQuoteStyle.Whole ? "\\\"" : "";
                                string sep1 = syntaxToken.QuoteStyle == SyntaxQuoteStyle.Value ? "\\\"" : "";
                                iw.WriteLine($"arguments.AppendLiteral(\" {sep0}{syntaxToken.Value}{SymbolDisplay.FormatLiteral(parameter.CommandLine!, false)}{syntaxToken.Separator}{sep1}\");");
                                iw.WriteLine($"arguments.AppendFormatted({parameter.Name}.Value{(parameter.IsSecret ? ", \"?\"" : "")});");
                                if (syntaxToken.QuoteStyle != SyntaxQuoteStyle.Automatic)
                                {
                                    iw.WriteLine("arguments.AppendLiteral('\"');");
                                }
                            }
                            else
                            {
                                iw.WriteLine($"arguments.AppendLiteral(\" {syntaxToken.Value}{SymbolDisplay.FormatLiteral(parameter.CommandLine!, false)}\");");
                            }

                            iw.CloseBracket();
                        }
                        else
                        {
                            if (parameter.Type.IsNullable)
                            {
                                iw.WriteLine(parameter.Type.IsValueType && !parameter.Type.IsCollection ?
                                    $"if ({parameter.Name}.HasValue)" :
                                    $"if ({parameter.Name} != null)");
                                iw.OpenBracket();
                            }

                            string appendCommandLine;
                            if (parameter.CommandLine != null)
                            {
                                string sep0 = syntaxToken.QuoteStyle == SyntaxQuoteStyle.Whole ? "\\\"" : "";
                                string sep1 = syntaxToken.QuoteStyle == SyntaxQuoteStyle.Value ? "\\\"" : "";
                                appendCommandLine = $"arguments.AppendLiteral(\" {sep0}{syntaxToken.Value}{SymbolDisplay.FormatLiteral(parameter.CommandLine, false)}{syntaxToken.Separator}{sep1}\");";
                            }
                            else
                            {
                                appendCommandLine = syntaxToken.QuoteStyle == SyntaxQuoteStyle.Automatic ?
                                    "arguments.AppendLiteral(' ');" :
                                    "arguments.AppendLiteral(\" \\\"\");";
                            }

                            string extraCalls = "";
                            if (parameter.Type.IsEnum)
                            {
                                extraCalls = parameter.Type is { IsNullable: true, IsCollection: false } ? ".Value.ToValueString()" : ".ToValueString()";
                            }
                            else if (parameter.Type is { IsNullable: true, IsValueType: true, IsCollection: false })
                            {
                                extraCalls = ".Value";
                            }

                            if (syntaxToken.QuoteStyle != SyntaxQuoteStyle.Automatic)
                            {
                                extraCalls += parameter.IsSecret ? ", \"?\\\"\"" : ", \"\\\"\"";
                            }
                            else if (parameter.IsSecret)
                            {
                                extraCalls += ", \"?\"";
                            }

                            if (parameter.Type.IsList)
                            {
                                if (parameter.Join == null)
                                {
                                    iw.WriteLine($"for (int i = 0; i < {parameter.Name}.Count; ++i)");
                                    iw.OpenBracket();
                                    iw.WriteLine(appendCommandLine);
                                    iw.WriteLine($"arguments.AppendFormatted({parameter.Name}[i]{extraCalls});");
                                    WriteQuote2(iw, syntaxToken);
                                    iw.CloseBracket();
                                }
                                else
                                {
                                    iw.WriteLine(appendCommandLine);
                                    iw.WriteLine($"for (int i = 0; i < {parameter.Name}.Count; ++i)");
                                    iw.OpenBracket();
                                    iw.WriteLine("if (i > 0)");
                                    iw.OpenBracket();
                                    iw.WriteLine($"arguments.AppendLiteral({JoinToParam(parameter.Join)});");
                                    iw.CloseBracket();
                                    iw.WriteLine($"arguments.AppendFormatted({parameter.Name}[i]{extraCalls});");
                                    iw.CloseBracket();
                                    WriteQuote2(iw, syntaxToken);
                                }
                            }
                            else if (parameter.Type.IsDictionary)
                            {
                                if (parameter.Join == null)
                                {
                                    iw.WriteLine($"foreach (var kv in {parameter.Name})");
                                    iw.OpenBracket();
                                    iw.WriteLine(appendCommandLine);
                                    iw.WriteLine("arguments.AppendFormatted(kv.Key);");
                                    iw.WriteLine("arguments.AppendLiteral('=');");
                                    iw.WriteLine($"arguments.AppendFormatted(kv.Value{extraCalls});");
                                    WriteQuote2(iw, syntaxToken);
                                    iw.CloseBracket();
                                }
                                else
                                {
                                    iw.WriteLine(appendCommandLine);
                                    iw.WriteLine($"bool needSeparatorFor{parameter.Name} = false;");
                                    iw.WriteLine($"foreach (var kv in {parameter.Name})");
                                    iw.OpenBracket();
                                    iw.WriteLine($"if (needSeparatorFor{parameter.Name})");
                                    iw.OpenBracket();
                                    iw.WriteLine($"arguments.AppendLiteral({JoinToParam(parameter.Join)});");
                                    iw.CloseBracket();
                                    iw.WriteLine("arguments.AppendFormatted(kv.Key);");
                                    iw.WriteLine("arguments.AppendLiteral('=');");
                                    iw.WriteLine($"arguments.AppendFormatted(kv.Value{extraCalls});");
                                    iw.WriteLine($"needSeparatorFor{parameter.Name} = true;");
                                    iw.CloseBracket();
                                    WriteQuote2(iw, syntaxToken);
                                }
                            }
                            else
                            {
                                iw.WriteLine(appendCommandLine);
                                iw.WriteLine($"arguments.AppendFormatted({parameter.Name}{extraCalls});");
                                WriteQuote2(iw, syntaxToken);
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

        static void WriteQuote2(IndentedTextWriter iw, SyntaxToken syntaxToken)
        {
            if (syntaxToken.QuoteStyle != SyntaxQuoteStyle.Automatic)
            {
                iw.WriteLine("arguments.AppendLiteral('\"');");
            }
        }

        static string JoinToParam(string join)
        {
            //TODO: Escape of char ...
            return join.Length == 1 ? $"'{join[0]}'" : SymbolDisplay.FormatLiteral(join, true);
        }
    }

    private static void GenerateBuilderParameter(IndentedTextWriter iw, Parameter parameter, bool isNew)
    {
        string optionalTypeName = parameter.Type is { IsBool: true, IsNullable: false } ?
            "bool" :
            parameter.Type.AsNullable();

        if (!isNew)
        {
            if (parameter.Type.IsList)
            {
                iw.WriteLine($"public System.Collections.Generic.List<{parameter.Type.Name}> {parameter.Name} {{ get; }}");
            }
            else if (parameter.Type.IsDictionary)
            {
                iw.WriteLine($"public System.Collections.Generic.Dictionary<string, {parameter.Type.Name}> {parameter.Name} {{ get; }}");
            }
            else
            {
                iw.WriteLine($"public {optionalTypeName} {parameter.Name} {{ get; set; }}");
            }

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
        else if (parameter.Type.IsList)
        {
            iw.WriteLine($"public {prefix}Builder Add{parameter.Name}(params {parameter.Type.Name}[] values)");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name}.AddRange(values);");
            iw.WriteLine("return this;");
            iw.CloseBracket();
            iw.WriteLine();
            iw.WriteLine($"public {prefix}Builder Clear{parameter.Name}()");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name}.Clear();");
            iw.WriteLine("return this;");
            iw.CloseBracket();
        }
        else if (parameter.Type.IsDictionary)
        {
            iw.WriteLine($"public {prefix}Builder Add{parameter.Name}(string key, {parameter.Type.Name} value)");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name}.Add(key, value);");
            iw.WriteLine("return this;");
            iw.CloseBracket();
            iw.WriteLine();
            iw.WriteLine($"public {prefix}Builder Clear{parameter.Name}()");
            iw.OpenBracket();
            iw.WriteLine($"{parameter.Name}.Clear();");
            iw.WriteLine("return this;");
            iw.CloseBracket();
        }
        else
        {
            iw.WriteLine($"public {prefix}Builder Set{parameter.Name}({optionalTypeName} value)");
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

    private static (string? CommandLine, bool IsSecret, string? Join) GetParameterArguments(
        SyntaxList<AttributeListSyntax> attributeList,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        if (TryGetAttributeConstructorArguments(
                attributeList,
                a => a.Name.ToString() is "Parameter" or "ParameterAttribute",
                semanticModel,
                cancellationToken,
                out var results))
        {
            return ((string?)results[0], (bool)results[1]!, (string?)results[2]);
        }

        return default;
    }

    private static bool TryGetAttributeConstructorArguments(
        SyntaxList<AttributeListSyntax> attributeList,
        Func<AttributeSyntax, bool> predicate,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        [NotNullWhen(true)] out object?[]? results)
    {
        var attribute = attributeList
            .SelectMany(static a => a.Attributes)
            .FirstOrDefault(predicate);

        if (attribute == null)
        {
            results = null;
            return false;
        }

        results = GetAttributeConstructorArguments(attribute, semanticModel, cancellationToken);
        return true;
    }

    private static object?[] GetAttributeConstructorArguments(
        AttributeSyntax attribute,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var attributeConstructor = (IMethodSymbol?)semanticModel.GetSymbolInfo(attribute).Symbol;
        var parameters = attributeConstructor.Parameters;
        var arguments = attribute.ArgumentList.Arguments;
        var results = new object?[parameters.Length];
        var parameterIndexes = new Dictionary<string, int>();
        for (int i = 0; i < parameters.Length; ++i)
        {
            if (parameters[i].HasExplicitDefaultValue)
            {
                results[i] = parameters[i].ExplicitDefaultValue;
            }

            parameterIndexes.Add(parameters[i].Name, i);
        }

        for (int i = 0; i < arguments.Count; ++i)
        {
            var arg = arguments[i];
            var value = semanticModel.GetConstantValue(arg.Expression, cancellationToken);
            if (!value.HasValue)
            {
                continue;
            }

            if (arg.NameColon == null)
            {
                results[i] = value.Value;
            }
            else
            {
                results[parameterIndexes[arg.NameColon.Name.ToString()]] = value.Value;
            }
        }

        return results;
    }
}