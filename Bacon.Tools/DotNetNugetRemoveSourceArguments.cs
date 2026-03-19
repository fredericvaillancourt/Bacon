using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} remove source {args}")]
public abstract partial class DotNetNugetRemoveSourceArguments : DotNetNugetArguments
{
    public string Name { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}