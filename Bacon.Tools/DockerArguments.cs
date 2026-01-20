using Bacon.Build;

namespace Bacon.Tools;

[Tool("docker")]
[Syntax("{args}")]
public abstract partial class DockerArguments : Arguments
{
}