using Bacon.Build;

namespace Bacon.Tools;

[Syntax("solution {args}")]
public abstract partial class DotNetSolutionArguments : DotNetArguments
{
    public string? SolutionFile { get; }
}