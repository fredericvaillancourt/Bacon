using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} clean {args}")]
public partial class DotNetWorkloadCleanArguments : DotNetWorkloadArguments
{
    [Parameter("all")]
    public bool All { get; }
}