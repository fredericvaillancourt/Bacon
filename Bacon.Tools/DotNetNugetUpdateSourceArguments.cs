using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} source {args}")]
public partial class DotNetNugetUpdateSourceArguments : DotNetNugetUpdateArguments
{
    public string Name { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("password", true)]
    public string? Password { get; }

    [Parameter("store-password-in-clear-text")]
    public bool StorePasswordInClearText { get; }

    [Parameter("valid-authentication-types", join: ",")]
    public IReadOnlyList<string>? BalidAuthenticationTypes { get; }

    [Parameter("protocol-version")]
    public int? ProtocolVersion { get; }

    [Parameter("allow-insecure-connections")]
    public bool AllowInsecureConnections { get; }
}