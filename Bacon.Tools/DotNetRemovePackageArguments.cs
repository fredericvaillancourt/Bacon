using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} package {args}")]
public partial class DotNetRemovePackageArguments : DotNetRemoveArguments
{
    public string Package { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }
}