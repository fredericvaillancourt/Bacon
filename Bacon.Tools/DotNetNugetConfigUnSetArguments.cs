using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} config unset {args}")]
public partial class DotNetNugetConfigUnSetArguments : DotNetNugetArguments
{
    public string Key { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}