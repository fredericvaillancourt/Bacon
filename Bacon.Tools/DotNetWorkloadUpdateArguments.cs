using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} update {args}")]
public partial class DotNetWorkloadUpdateArguments : DotNetWorkloadArguments
{
    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("include-previews")]
    public bool IncludePreviews { get; }

    [Parameter("temp-dir")]
    public string? TempDir { get; }

    [Parameter("from-previous-sdk")]
    public bool FromPreviousSdk { get; }

    [Parameter("advertising-manifests-only")]
    public bool AdvertisingManifestsOnly { get; }

    [Parameter("version")]
    public string? Version { get; }

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

    [Parameter("from-history")]
    public string? FromHistory { get; }

    [Parameter("manifests-only")]
    public string? ManifestsOnly { get; }
}