using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} build {args}")]
public partial class DotNetBuildArguments : DotNetArguments
{
    //TODO: No attributes to say it is "filenames" or should we have one, maybe for positional when copy from to?
    public string Target { get; }

    [Parameter("use-current-runtime")]
    public bool UseCurrentRuntime { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("version-suffix")]
    public string? VersionSuffix { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("debug")]
    public bool Debug { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }

    [Parameter("no-incremental")]
    public bool NoIncremental { get; }

    [Parameter("no-dependencies")]
    public bool NoDependencies { get; }

    [Parameter("nologo")]
    public bool NoLogo { get; }

    [Parameter("self-contained")]
    public bool SelfContained { get; }

    [Parameter("no-self-contained")]
    public bool NoSelfContained { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("os")]
    public string? OperatingSystem { get; }

    [Parameter("disable-build-servers")]
    public bool DisableBuildServers { get; }
}