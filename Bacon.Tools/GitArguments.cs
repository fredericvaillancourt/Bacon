using Bacon.Build;

namespace Bacon.Tools;

[Tool("git")]
[Syntax("{args}")]
public abstract partial class GitArguments : Arguments
{
}