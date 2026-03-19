using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} repair {args}")]
public partial class DotNetWorkloadRepairArguments : DotNetWorkloadArguments
{
    public string WorkloadId { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("disable-parallel")]
    public bool DisableParallel { get; }

    [Parameter("ignore-failed-sources")]
    public bool IgnoreFailedSources { get; }

    [Parameter("no-http-cache")]
    public bool NoHttpCache { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }
}