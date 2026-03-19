namespace Bacon.Generator;

internal sealed record ClassInfo(
    string? Namespace,
    string ClassName,
    string FullClassName,
    bool IsAbstract,
    EquatableArray<Parameter> Parameters
)
{
    public static ClassInfo From(ArgumentsPreInfo preInfo)
    {
        return new ClassInfo(
            preInfo.Namespace,
            preInfo.ClassName,
            preInfo.FullClassName,
            preInfo.IsAbstract,
            preInfo.Parameters);
    }
}