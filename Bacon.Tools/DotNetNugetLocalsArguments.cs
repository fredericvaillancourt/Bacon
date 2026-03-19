using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} locals {args}")]
public partial class DotNetNugetLocalsArguments : DotNetNugetArguments
{
    public DotNetNugetLocalsLocation Location { get; }

    [Parameter("force-english-output")]
    public bool ForceEnglishOutput { get; }

    [Parameter("clear")]
    public bool Clear { get; }

    [Parameter("list")]
    public bool List { get; }
}