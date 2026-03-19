using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} add {args}")]
public partial class DotNetReferenceAddArguments : DotNetReferenceArguments
{
    public string ProjectPath { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("project")]
    public string? Project { get; }
}