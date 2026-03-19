using Bacon.Build;

namespace Bacon.Tests;

[Syntax("req {args}")]
public partial class TestRequiredArguments : TestArguments
{
    public string Required { get; }
}