using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} install {args}")]
public partial class DotNetWorkloadInstallArguments : DotNetWorkloadArguments
{
    public string WorkloadId { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("include-previews")]
    public bool IncludePreviews { get; }

    [Parameter("skip-manifest-update")]
    public bool SkipManifestUpdate { get; }

    [Parameter("temp-dir")]
    public string? TempDir { get; }

    [Parameter("disable-parallel")]
    public bool DisableParallel { get; }

    [Parameter("ignore-failed-sources")]
    public bool IgnoreFailedSources { get; }

    [Parameter("no-http-cache")]
    public bool NoHttpCache { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("version")]
    public string? Version { get; }
}