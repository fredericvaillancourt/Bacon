using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} enable source {args}")]
public partial class DotNetNugetEnableSourceArguments : DotNetNugetArguments
{
    public string Name { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}