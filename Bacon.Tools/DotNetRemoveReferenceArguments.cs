using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} reference {args}")]
public partial class DotNetRemoveReferenceArguments : DotNetRemoveArguments
{
    public string Reference { get; }

    [Parameter("framework")]
    public string? Framework { get; }
}