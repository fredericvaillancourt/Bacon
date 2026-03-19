using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} update {args}")]
public partial class DotNetPackageUpdateArguments : DotNetPackageArguments
{
    //TODO: Should we just allow null in parameter name? Could make sure secret works too ...
    [Parameter(null!, join: " ")]
    public IReadOnlyList<string>? Packages { get; }

    [Parameter("vulnerable")]
    public bool Bulnerable { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}