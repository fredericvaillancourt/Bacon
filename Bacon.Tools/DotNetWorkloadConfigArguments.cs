using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} config {args}")]
public partial class DotNetWorkloadConfigArguments : DotNetWorkloadArguments
{
    [Parameter("update-mode")]
    public DotNetWorkloadConfigUpdateMode? UpdateMode { get; }
}