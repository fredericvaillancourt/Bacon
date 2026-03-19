using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} search {args}")]
public partial class DotNetPackageSearchArguments : DotNetPackageArguments
{
    public string SearchTerm { get; }

    [Parameter("source")]
    public string? Source { get; }

    [Parameter("take")]
    public int? Take { get; }

    [Parameter("skip")]
    public int? Skip { get; }

    [Parameter("exact-match")]
    public bool ExactMatch { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("prerelease")]
    public bool PreRelease { get; }

    [Parameter("configfile")]
    public string? ConfigFile { get; }

    [Parameter("format")]
    public DotNetOutputFormat? Format { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }
}