using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list {args}")]
public partial class DotNetSolutionListArguments : DotNetSolutionArguments
{
    [Parameter("solution-folder")]
    public string? SolutionFolder { get; }
}