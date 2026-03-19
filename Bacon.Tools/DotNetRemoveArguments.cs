using Bacon.Build;

namespace Bacon.Tools;

[Syntax("remove {args}")]
public abstract partial class DotNetRemoveArguments : DotNetArguments
{
    public string Project { get; }
}