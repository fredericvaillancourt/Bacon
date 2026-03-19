using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list {args}")]
public partial class DotNetReferenceListArguments : DotNetReferenceArguments
{
    [Parameter("project")]
    public string? Project { get; }
}