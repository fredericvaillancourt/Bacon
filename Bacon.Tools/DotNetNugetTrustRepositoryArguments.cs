using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust repository {args}")]
public partial class DotNetNugetTrustRepositoryArguments : DotNetNugetArguments
{
    public string Name { get; }
    public string Package { get; }

    [Parameter("allow-untrusted-root")]
    public bool AllowUntrustedRoot { get; }

    [Parameter("owners")]
    public string? Owners { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}