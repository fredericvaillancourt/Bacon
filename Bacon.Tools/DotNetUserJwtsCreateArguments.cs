using Bacon.Build;

namespace Bacon.Tools;

[Syntax("user-jwts create {args}")]
public partial class DotNetUserJwtsCreateArguments : DotNetArguments
{
    [Parameter("project")]
    public string? Project { get; }

    [Parameter("output")]
    public DotNetUserJwtsOutput? Output { get; }

    [Parameter("scheme")]
    public string? Scheme { get; }

    [Parameter("name")]
    public string? Name { get; }

    [Parameter("audiance")]
    public string? Audiance { get; }

    [Parameter("issuer")]
    public string? Issuer { get; }

    [Parameter("scope")]
    public string? Scope { get; }

    [Parameter("role")]
    public string? Role { get; }

    [Parameter("claim")]
    public IReadOnlyDictionary<string, string> Claims { get; }

    [Parameter("not-before")]
    public string? NotBefore { get; } //TODO: Thoses could be datetime ... but would need custom format ... we almost can do it.

    [Parameter("expires-on")]
    public string? ExpiresOn { get; }

    [Parameter("valid-for")]
    public string? ValidFor { get; } //TODO: Could be some custom formating trick too

    [Parameter("appsettings-file")]
    public string? AppSettingsFile { get; }
}