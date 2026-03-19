using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} config paths {args}")]
public partial class DotNetNugetConfigPathsArguments : DotNetNugetArguments
{
    [Parameter("working-directory")]
    public string? WorkingDirectory { get; }
}