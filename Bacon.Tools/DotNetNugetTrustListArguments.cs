using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust list {args}")]
public partial class DotNetNugetTrustListArguments : DotNetNugetArguments
{

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}