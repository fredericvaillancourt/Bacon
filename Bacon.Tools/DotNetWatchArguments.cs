using Bacon.Build;

namespace Bacon.Tools;

[Syntax("watch {args}")]
public partial class DotNetWatchArguments : DotNetArguments
{
    public string Command { get; }

    [Parameter("quiet")]
    public bool Quiet { get; }

    [Parameter("verbose")]
    public bool Verbose { get; }

    [Parameter("list")]
    public bool List { get; }

    [Parameter("no-hot-reload")]
    public bool NoHotReload { get; }

    [Parameter("non-interactive")]
    public bool NonInteractive { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("self-contained")]
    public bool SelfContained { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("os")]
    public string? OperatingSystem { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }
}