using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} remove client-cert {args}")]
public abstract partial class DotNetNugetRemoveClientCertArguments : DotNetNugetArguments
{
    [Parameter("package-source")]
    public string PackageSource { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}