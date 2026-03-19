using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list {args}")]
public partial class DotNetPackageListArguments : DotNetPackageArguments
{
    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("outdated")]
    public bool Outdated { get; }

    [Parameter("deprecated")]
    public bool Deprecated { get; }

    [Parameter("vulnerable")]
    public bool Vulnerable { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("include-transitive")]
    public bool IncludeTransitive { get; }

    [Parameter("include-prerelease")]
    public bool IncludePrerelease { get; }

    [Parameter("highest-patch")]
    public bool HighestPatch { get; }

    [Parameter("highest-minor")]
    public bool HighestMinor { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("format")]
    public DotNetOutputFormat? Format { get; }

    [Parameter("output-version")]
    public string? OutputVersion { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("project")]
    public string? Project { get; }
}