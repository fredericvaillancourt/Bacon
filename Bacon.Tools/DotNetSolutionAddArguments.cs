using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} add {args}")]
public partial class DotNetSolutionAddArguments : DotNetSolutionArguments
{
    public string ProjectPath { get; }

    [Parameter("in-root")]
    public bool InRoot { get; }

    [Parameter("solution-folder")]
    public string? SolutionFolder { get; }

    [Parameter("include-references")]
    public bool? IncludeReferences { get; }
}