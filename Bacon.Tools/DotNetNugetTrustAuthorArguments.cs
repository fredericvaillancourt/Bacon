using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} trust author {args}")]
public partial class DotNetNugetTrustAuthorArguments : DotNetNugetArguments
{
    public string Name { get; }
    public string Author { get; }

    [Parameter("allow-untrusted-root")]
    public bool AllowUntrustedRoot { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}