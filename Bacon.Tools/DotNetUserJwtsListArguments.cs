using Bacon.Build;

namespace Bacon.Tools;

[Syntax("user-jwts list {args}")]
public partial class DotNetUserJwtsListArguments : DotNetArguments
{
    [Parameter("project")]
    public string? Project { get; }

    [Parameter("output")]
    public DotNetUserJwtsOutput? Output { get; }

    [Parameter("show-tokens")]
    public bool ShowTokens { get; }
}