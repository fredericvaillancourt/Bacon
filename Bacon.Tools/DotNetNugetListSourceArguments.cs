using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list source {args}")]
public partial class DotNetNugetListSourceArguments : DotNetNugetArguments
{
    [Parameter("format")]
    public DotNetNugetListSourceFormat? Format { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}