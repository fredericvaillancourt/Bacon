using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} uninstall {args}")]
public partial class DotNetWorkloadUninstallArguments : DotNetWorkloadArguments
{
    public string WorkloadId { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}