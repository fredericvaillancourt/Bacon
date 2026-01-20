using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} push {args}")]
public partial class OctopusPushArguments : OctopusArguments
{
    [Parameter("package")]
    public string[] Packages { get; }

    [Parameter("overwrite-mode")]
    public OctopusPushOverwriteMode? OverwriteMode { get; }

    [Parameter("replace-existing")]
    public bool ReplaceExisting { get; }

    [Parameter("use-delta-compression")]
    public bool UseDeltaCompression { get; } //TODO: Help says we should do --use-delta-compression true ... can we just do --use-delta-compression?

    [Parameter("server")]
    public string? Server { get; }

    [Parameter("apiKey")]
    public string? ApiKey { get; }

    [Parameter("user")]
    public string? User { get; }

    [Parameter("pass")]
    public string? Password { get; }

    [Parameter("configFile")]
    public string? ConfigurationFile { get; }

    [Parameter("debug")]
    public bool Debug { get; }

    [Parameter("ignoreSslErrors")]
    public bool IgnoreSslErrors { get; }

    [Parameter("enableServiceMessages")]
    public bool EnableServiceMessages { get; }

    [Parameter("timeout")]
    public int? Timeout { get; }

    [Parameter("proxy")]
    public string? Proxy { get; }

    [Parameter("proxyYser")]
    public string? ProxyUser { get; }

    [Parameter("proxyPass")]
    public string? ProxyPassword { get; }

    [Parameter("space")]
    public string? Space { get; }
}