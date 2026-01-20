using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} publish {args}")]
public partial class DotNetPublishArguments : DotNetArguments
{
    //TODO: No attributes to say it is "filenames" or should we have one, maybe for positional when copy from to?
    public string Target { get; }

    [Parameter("use-current-runtime")]
    public bool UseCurrentRuntime { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }

    [Parameter("manifest")]
    public string? Manifest { get; }

    [Parameter("no-build")]
    public bool NoBuild { get; }

    [Parameter("self-contained")]
    public bool SelfContained { get; }

    [Parameter("no-self-contained")]
    public bool NoSelfContained { get; }

    [Parameter("nologo")]
    public bool NoLogo { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("version-suffix")]
    public string? VersionSuffix { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("os")]
    public string? OperatingSystem { get; }

    [Parameter("disable-build-servers")]
    public bool DisableBuildServers { get; }
}