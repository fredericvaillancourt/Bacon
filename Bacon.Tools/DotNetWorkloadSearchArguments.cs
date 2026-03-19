using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} search {args}")]
public partial class DotNetWorkloadSearchArguments : DotNetWorkloadArguments
{
    public string? SearchString { get; }
}