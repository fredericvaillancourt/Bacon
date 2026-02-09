using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} push {args}")]
public partial class DotNetNugetPushArguments : DotNetNugetArguments
{
    public string Package { get; }

    [Parameter("allow-insecure-connections")]
    public bool AllowInsecureConnections { get; }

    [Parameter("disable-buffering")]
    public bool DisableBuffering { get; }

    [Parameter("force-english-output")]
    public bool ForceEnglishOutput { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("api-key")]
    public string? ApiKey { get; }

    [Parameter("no-symbols")]
    public bool NoSymbols { get; }

    [Parameter("no-service-endpoint")]
    public bool NoServiceEndpoint { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("skip-duplicate")]
    public bool SkipDuplicate { get; }

    [Parameter("symbol-api-key")]
    public string? SymbolApiKey { get; }

    [Parameter("symbol-source")]
    public string? SymbolSource { get; }

    [Parameter("timeout")]
    public int? Timeout { get; }

    [Parameter("configfile")]
    public string? Configfile { get; }
}