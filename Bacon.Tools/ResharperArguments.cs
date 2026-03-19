using Bacon.Build;

namespace Bacon.Tools;

[Tool("JetBrains.ReSharper.GlobalTools", ToolLocation.Tool)]
[Syntax("")]
public abstract partial class ResharperArguments : Arguments
{
}