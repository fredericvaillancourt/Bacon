using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} uninstall {args}")]
public partial class DotNetToolUnInstallArguments : DotNetToolArguments
{
    public string PackageId { get; }

    [Parameter("global")]
    public bool Global { get; }

    [Parameter("local")]
    public bool Local { get; }

    [Parameter("tool-path")]
    public string? ToolPath { get; }

    [Parameter("tool-manifest")]
    public string? ToolManifest { get; }
}