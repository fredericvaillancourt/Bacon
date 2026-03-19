using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} restore {args}")]
public partial class DotNetWorkloadRestoreArguments : DotNetWorkloadArguments
{
    public string Target { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("include-previews")]
    public bool IncludePreviews { get; }

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
}