using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} install {args}")]
public partial class DotNetToolInstallArguments : DotNetToolArguments
{
    public string PackageId { get; }

    [Parameter("global")]
    public bool Global { get; }

    [Parameter("local")]
    public bool Local { get; }

    [Parameter("tool-path")]
    public string? ToolPath { get; }

    [Parameter("version")]
    public string? Version { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("tool-manifest")]
    public string? ToolManifest { get; }

    [Parameter("add-source")]
    public string? AddSource { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("prerelease")]
    public bool PreRelease { get; }

    [Parameter("disable-parallel")]
    public bool DisableParallel { get; }

    [Parameter("ignore-failed-sources")]
    public bool IgnoreFailedSources { get; }

    [Parameter("no-http-cache")]
    public bool NoHttpCache { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("create-manifest-if-needed")]
    public bool CreateManifestIfNeeded { get; }

    [Parameter("allow-downgrade")]
    public bool AllowDowngrade { get; }

    [Parameter("allow-roll-forward")]
    public bool AllowRollForward { get; }
}