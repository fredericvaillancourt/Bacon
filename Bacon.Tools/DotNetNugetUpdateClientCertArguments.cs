using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} client-cert {args}")]
public partial class DotNetNugetUpdateClientCertArguments : DotNetNugetUpdateArguments
{
    [Parameter("package-source")]
    public string? PackageSource { get; }

    [Parameter("path")]
    public string? Path { get; }

    [Parameter("password", true)]
    public string? Password { get; }

    [Parameter("store-password-in-clear-text")]
    public bool StorePasswordInClearText { get; }

    [Parameter("store-location")]
    public string? StoreLocation { get; }

    [Parameter("store-name")]
    public string? StoreName { get; }

    [Parameter("find-by")]
    public string? FindBy { get; }

    [Parameter("find-value")]
    public string? FindValue { get; }

    [Parameter("force")]
    public bool Force { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}