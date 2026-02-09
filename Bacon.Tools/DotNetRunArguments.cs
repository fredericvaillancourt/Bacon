using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} run {args}")]
public partial class DotNetRunArguments : DotNetArguments
{
    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("file")]
    public string? File { get; }

    [Parameter("launch-profile")]
    public string? LaunchProfile { get; }

    [Parameter("no-launch-profile")]
    public bool NoLaunchProfile { get; }

    [Parameter("no-build")]
    public bool NoBuild { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("no-cache")]
    public bool NoCache { get; }

    [Parameter("self-contained")]
    public bool SelfContained { get; }

    [Parameter("no-self-contained")]
    public bool NoSelfContained { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("os")]
    public string? Os { get; }

    [Parameter("disable-build-server")]
    public bool DisableBuildServer { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }

    [Parameter("environment")]
    public IReadOnlyDictionary<string, string>? Environments { get; }
}