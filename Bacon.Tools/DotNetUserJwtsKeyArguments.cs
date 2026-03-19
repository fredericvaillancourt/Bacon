using Bacon.Build;

namespace Bacon.Tools;

[Syntax("user-jwts key {args}")]
public partial class DotNetUserJwtsKeyArguments : DotNetArguments
{
    [Parameter("project")]
    public string? Project { get; }

    [Parameter("output")]
    public DotNetUserJwtsOutput? Output { get; }

    [Parameter("scheme")]
    public string? Scheme { get; }

    [Parameter("issuer")]
    public string? Issuer { get; }

    [Parameter("reset")]
    public bool Reset { get; }

    [Parameter("force")]
    public bool Force { get; }
}