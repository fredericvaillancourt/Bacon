using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} delete {args}")]
public partial class DotNetNugetDeleteArguments : DotNetNugetArguments
{
    [Parameter("force-english-output")]
    public bool ForceEnglishOutput { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("non-interactive")]
    public bool NonInteractive { get; }

    [Parameter("api-key", true)]
    public string? ApiKey { get; }

    [Parameter("no-service-endpoint")]
    public bool NoServiceEndpoint { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }
}