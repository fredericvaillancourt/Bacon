using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} sign {args}")]
public partial class DotNetNugetSignArguments : DotNetNugetArguments
{
    public string Path { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("certificate-path")]
    public string? CertificatePath { get; }

    [Parameter("certificate-store-name")]
    public string? CertificateStoreName { get; }

    [Parameter("certificate-store-location")]
    public string? CertificateStoreLocation { get; }

    [Parameter("certificate-subject-name")]
    public string? CertificateSubjectName { get; }

    [Parameter("certificate-fingerprint")]
    public string? CertificateFingerprint { get; }

    [Parameter("certificate-password", true)]
    public string? CertificatePassword { get; }

    [Parameter("hash-algorithm")]
    public string? HashAlgorithm { get; }

    [Parameter("timestamper")]
    public string? Timestamper { get; }

    [Parameter("timestamp-hash-algorithm")]
    public string? TimestampHashAlgorithm { get; }

    [Parameter("overwrite")]
    public bool Overwrite { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}