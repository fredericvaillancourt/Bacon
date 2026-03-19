using Bacon.Build;

namespace Bacon.Tools;

[Syntax("user-jwts print {args}")]
public partial class DotNetUserJwtsPrintArguments : DotNetArguments
{
    public string Id { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("output")]
    public DotNetUserJwtsOutput? Output { get; }

    [Parameter("show-all")]
    public bool ShowAll { get; }
}