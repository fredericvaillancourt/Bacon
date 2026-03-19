using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} search {args}")]
public partial class DotNetToolSearchArguments : DotNetToolArguments
{
    public string SearchTerm { get; }

    [Parameter("detail")]
    public bool Detail { get; }

    [Parameter("skip")]
    public int? Skip { get; }

    [Parameter("take")]
    public int? Take { get; }

    [Parameter("prerelease")]
    public bool PreRelease { get; }
}