namespace Bacon.Generator;

internal sealed record ArgumentsInfo(
    ClassInfo ClassInfo,
    ClassInfo? ParentClassInfo,
    EquatableArray<Parameter> AllParentParameters,
    string Syntax,
    string ToolName,
    string ActionName)
{
    public static ArgumentsInfo From(ArgumentsPreInfoGrouping group)
    {
        var pre = group.ArgumentsPreInfo;
        return new(
            group.ClassInfo,
            group.Base?.ClassInfo,
            group.BaseParameters,
            pre.Syntax,
            pre.ToolName,
            pre.ActionName);
    }

    public bool HasRequiredParameters => ClassInfo.Parameters.Any(IsRequired) || AllParentParameters.Any(IsRequired);

    private static bool IsRequired(Parameter parameter) => parameter.Type is { IsNullable: false, IsBool: false };
}