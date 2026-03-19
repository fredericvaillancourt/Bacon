using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} remove {args}")]
public partial class DotNetPackageRemoveArguments : DotNetPackageArguments
{
    public string PackageName { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("file")]
    public string? File { get; }
}