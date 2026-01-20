using Bacon.Build;

namespace Bacon.Tools;

[Tool("npx")]
[Syntax("{args}")]
public abstract partial class NpxArguments : Arguments
{
    [Parameter("yes")]
    public bool Yes { get; }

    public string Package { get; }
}