using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} version {args}")]
public partial class DotNetWorkloadSearchVersionArguments : DotNetWorkloadSearchArguments
{
    public string? WorkloadVersion { get; }

    [Parameter("format")]
    public DotNetWorkloadSearchFormat? Format { get; }

    [Parameter("take")]
    public int? Take { get; }

    [Parameter("include-previews")]
    public bool IncludePreviews { get; }
}