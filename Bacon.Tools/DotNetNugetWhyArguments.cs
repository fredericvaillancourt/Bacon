using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} why {args}")]
public partial class DotNetNugetWhyArguments : DotNetNugetArguments
{
    public string Target { get; }
    public string Package { get; }

    [Parameter("framework")]
    public string? Framework { get; }
}