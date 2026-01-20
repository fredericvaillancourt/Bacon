using Bacon.Build;

namespace Bacon.Tools;

[Tool("JetBrains.ReSharper.GlobalTools", ToolLocation.Tool)]
[Syntax("{args}")]
public abstract partial class ResharperArguments : Arguments
{
}