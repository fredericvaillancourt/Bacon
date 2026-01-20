using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} push {args}")]
public partial class DockerPushArguments : DockerArguments
{
    [Parameter("all-tags")]
    public bool AllTags { get; }

    public string Name { get; }
}