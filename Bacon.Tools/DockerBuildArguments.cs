using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} build {args}")]
public partial class DockerBuildArguments : DockerArguments
{
    [Parameter("file")]
    public string? File { get; }

    [Parameter("tag")]
    public IReadOnlyList<string>? Tags { get; }

    [Parameter("target")]
    public string? Target { get; }

    public string Path { get; }
}