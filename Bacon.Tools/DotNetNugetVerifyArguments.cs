using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} verify {args}")]
public partial class DotNetNugetVerifyArguments : DotNetNugetArguments
{
    public string PackagePaths { get; }

    [Parameter("all")]
    public bool All { get; }

    [Parameter("certificate-fingerprint")]
    public string? CertificateFingerprint { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}