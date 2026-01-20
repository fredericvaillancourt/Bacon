using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} tool restore {args}")]
public partial class DotNetToolRestoreArguments : DotNetArguments
{
    [Parameter("configFile")]
    public string? ConfigFile { get; }

    [Parameter("add-source")]
    public string? AddSource { get; }

    [Parameter("tool-manifest")]
    public string? ToolManifest { get; }

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
}