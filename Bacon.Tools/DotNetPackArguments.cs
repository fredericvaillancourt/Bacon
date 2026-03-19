using Bacon.Build;

namespace Bacon.Tools;

[Syntax("pack {args}")]
public partial class DotNetPackArguments : DotNetArguments
{
    public string Target { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }

    [Parameter("no-build")]
    public bool NoBuild { get; }

    [Parameter("include-symbols")]
    public bool IncludeSymbols { get; }

    [Parameter("include-source")]
    public bool IncludeSource { get; }

    [Parameter("serviceable")]
    public bool Serviceable { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("version-suffix")]
    public string? VersionSuffix { get; }

    [Parameter("version")]
    public string? Version { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("disable-build-servers")]
    public bool DisableBuildServers { get; }

    [Parameter("use-current-runtime")]
    public bool UseCurrentRuntime { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }
}