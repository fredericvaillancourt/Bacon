using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} remove {args}")]
public partial class DotNetSolutionRemoveArguments : DotNetSolutionArguments
{
    public string ProjectPath { get; }
}