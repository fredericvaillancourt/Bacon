using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list {args}")]
public partial class DotNetToolListArguments : DotNetToolArguments
{
    public string? PackageId { get; }

    [Parameter("global")]
    public bool Global { get; }

    [Parameter("local")]
    public bool Local { get; }

    [Parameter("tool-path")]
    public string? ToolPath { get; }

    [Parameter("format")]
    public DotNetOutputFormat? Format { get; }
}