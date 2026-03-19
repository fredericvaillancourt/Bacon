using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} config get {args}")]
public partial class DotNetNugetConfigGetArguments : DotNetNugetArguments
{
    public string Key { get; }

    [Parameter("working-directory")]
    public string? WorkingDirectory { get; }

    [Parameter("show-path")]
    public bool ShowPath { get; }
}