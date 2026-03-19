using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list client-cert {args}")]
public partial class DotNetNugetListClientCertArguments : DotNetNugetArguments
{
    [Parameter("configfile")]
    public string? ConfigFile { get; }
}