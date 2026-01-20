using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} clean {args}")]
public partial class DotNetCleanArguments : DotNetArguments
{
    public string Target { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }

    [Parameter("nologo")]
    public bool NoLogo { get; }

    [Parameter("disable-build-servers")]
    public bool DisableBuildServers { get; }
}