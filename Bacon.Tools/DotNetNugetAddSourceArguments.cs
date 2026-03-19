using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} add source {args}")]
public partial class DotNetNugetAddSourceArguments : DotNetNugetArguments
{
    [Parameter("package-source")]
    public string? PackageSourcePath { get; }

    [Parameter("name")]
    public string? Name { get; }

    [Parameter("username")]
    public string? Username { get; }

    [Parameter("password", true)]
    public string? Password { get; }

    [Parameter("store-password-in-clear-text")]
    public bool StorePasswordInClearText { get; }

    //TODO: Should this be an enum?
    [Parameter("valid-authentication-types")]
    public IReadOnlyList<string>? ValidAuthenticationTypes { get; }

    [Parameter("protocol-version")]
    public int? ProtocolVersion { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("allow-insecure-connections")]
    public bool AllowInsecureConnections { get; }
}