using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetNugetTrustCertificateAlgorithm
{
    [EnumName("SHA256")] Sha256,
    [EnumName("SHA384")] Sha384,
    [EnumName("SHA512")] Sha512,
}