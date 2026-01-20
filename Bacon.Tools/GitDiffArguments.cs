using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} diff {args}")]
public partial class GitDiffArguments : GitArguments
{
    [Parameter("name-only")]
    public bool NameOnly { get; }
}