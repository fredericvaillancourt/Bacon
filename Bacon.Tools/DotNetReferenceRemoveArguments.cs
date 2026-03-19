using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} remove {args}")]
public partial class DotNetReferenceRemoveArguments : DotNetReferenceArguments
{
    public string ProjectPath { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("project")]
    public string? Project { get; }
}