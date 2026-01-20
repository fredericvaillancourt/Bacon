using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} build-docs {args}")]
public partial class NpxRedoclyBuildDocsArguments : NpxRedoclyArguments
{
    public string InputPath { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("title")]
    public string? Title { get; }
}