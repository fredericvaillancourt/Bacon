using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust source {args}")]
public partial class DotNetNugetTrustSourceArguments : DotNetNugetArguments
{
    public string Name { get; }

    [Parameter("owners")]
    public string? Owners { get; }

    [Parameter("source-url")]
    public string? SourceUrl { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}