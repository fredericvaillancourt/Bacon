using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} config set {args}")]
public partial class DotNetNugetConfigSetArguments : DotNetNugetArguments
{
    public string Key { get; }
    public string Value { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}