using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} disable source {args}")]
public partial class DotNetNugetDisableSourceArguments : DotNetNugetArguments
{
    public string Name { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}