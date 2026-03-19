using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust remove {args}")]
public partial class DotNetNugetTrustRemoveArguments : DotNetNugetArguments
{
    public string Name { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}