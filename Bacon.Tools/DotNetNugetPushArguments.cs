using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} push {args}")]
public partial class DotNetNugetPushArguments : DotNetNugetArguments
{
    public string Package { get; }

    [Parameter("force-english-output")]
    public bool ForceEnglishOutput { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("allow-insecure-connections")]
    public bool AllowInsecureConnections { get; }

    [Parameter("symbol-source")]
    public string? SymbolSource { get; }

    [Parameter("timeout")]
    public int? Timeout { get; }

    [Parameter("api-key", true)]
    public string? ApiKey { get; }

    [Parameter("symbol-api-key", true)]
    public string? SymbolApiKey { get; }

    [Parameter("disable-buffering")]
    public bool DisableBuffering { get; }

    [Parameter("no-symbols")]
    public bool NoSymbols { get; }

    [Parameter("no-service-endpoint")]
    public bool NoServiceEndpoint { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("skip-duplicate")]
    public bool SkipDuplicate { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }
}