using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust certificate {args}")]
public partial class DotNetNugetTrustCertificateArguments : DotNetNugetArguments
{
    public string Name { get; }
    public string Fingerprint { get; }

    [Parameter("algorithm")]
    public DotNetNugetTrustCertificateAlgorithm? Algorithm { get; }

    [Parameter("allow-untrusted-root")]
    public bool AllowUntrustedRoot { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}