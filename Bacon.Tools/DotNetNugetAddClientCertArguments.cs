using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} add client-cert {args}")]
public partial class DotNetNugetAddClientCertArguments : DotNetNugetArguments
{
    [Parameter("package-source")]
    public string? PackageSource { get; }

    [Parameter("path")]
    public string? Path { get; }

    [Parameter("password", true)]
    public string? Password { get; }

    [Parameter("store-password-in-clear-text")]
    public bool StorePasswordInClearText { get; }

    //TODO: StoreLocation and StoreName
    //TODO: FindBy and FindValue

    [Parameter("force")]
    public bool Force { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}