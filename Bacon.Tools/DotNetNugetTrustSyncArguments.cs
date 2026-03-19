using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust sync {args}")]
public partial class DotNetNugetTrustSyncArguments : DotNetNugetArguments
{
    public string Name { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}