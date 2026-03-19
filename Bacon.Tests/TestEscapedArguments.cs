using Bacon.Build;

namespace Bacon.Tests;

[Syntax("esc {args}")]
public partial class TestEscapedArguments : TestArguments
{
    public string Escaped { get; }
}