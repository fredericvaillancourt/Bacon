using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} restore {args}")]
public partial class DotNetRestoreArguments : DotNetArguments
{
    //TODO: No attributes to say it is "filenames" or should we have one, maybe for positional when copy from to?
    public string Target { get; }

    [Parameter("disable-build-servers")]
    public bool DisableBuildServers { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("packages")]
    public string? Packages { get; }

    [Parameter("use-current-runtime")]
    public bool UseCurrentRuntime { get; }

    [Parameter("disable-parallel")]
    public bool DisableParallel { get; }

    [Parameter("configfile")]
    public string? ConfigurationFile { get; }

    [Parameter("no-http-cache")]
    public bool NoHttpCache { get; }

    [Parameter("ignore-failed-sources")]
    public bool IgnoreFailedSources { get; }

    [Parameter("force")]
    public bool Force { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("no-dependencies")]
    public bool NoDependencies { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactPath { get; }

    [Parameter("use-lock-file")]
    public bool UseLockFile { get; }

    [Parameter("locked-mode")]
    public bool LockedMode { get; }

    [Parameter("lock-file-path")]
    public string? LockFilePath { get; }

    [Parameter("force-evaluate")]
    public bool ForceEvaluate { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("os")]
    public string? OperatingSystem { get; }
}