using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} search {args}")]
public partial class DotNetPackageAddArguments : DotNetPackageArguments
{
    public string PackageId { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("package-directory")]
    public string? PackageDirectory { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("prerelease")]
    public bool PreRelease { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("file")]
    public string? File { get; }
}