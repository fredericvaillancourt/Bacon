using Bacon.Build;

namespace Bacon.Tools;

[Syntax("test {args}")]
public partial class DotNetTestVSTestArguments : DotNetArguments
{
    [Parameter("settings")]
    public string? Settings { get; }

    [Parameter("list-tests")]
    public bool ListTests { get; }

    [Parameter("environment")]
    public IReadOnlyDictionary<string, string>? Environments { get; }

    [Parameter("filter")]
    public string? Filter { get; }

    [Parameter("test-adapter-path")]
    public string? TestAdapterPath { get; }

    [Parameter("logger")]
    public string? Logger { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("artifacts-path")]
    public string? ArtifactsPath { get; }

    [Parameter("diag")]
    public string? Diagnostic { get; }

    [Parameter("no-build")]
    public bool NoBuild { get; }

    [Parameter("results-directory")]
    public string? ResultsDirectory { get; }

    [Parameter("collect")]
    public string? Collect { get; }

    [Parameter("blame")]
    public bool Blame { get; }

    [Parameter("blame-crash")]
    public bool BlameCrash { get; }

    [Parameter("blame-crash-dump-type")]
    public DotNetTestVSTestBlameCrashDumpType? BlameCrashDumpType { get; }

    [Parameter("blame-crash-collect-always")]
    public bool BlameCrashCollectAlways { get; }

    [Parameter("blame-hang")]
    public bool BlameHang { get; }

    [Parameter("blame-hang-dump-type")]
    public DotNetTestVSTestBlameCrashDumpType? BlameHangDumpType { get; }

    [Parameter("blame-hang-timeout")]
    public string? BlameHangTimeout { get; } //TODO: Timespan? Other time type?

    [Parameter("no-logo")]
    public bool NoLogo { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("os")]
    public string? OperatingSystem { get; }

    [Parameter("disable-build-servers")]
    public bool DisableBuildServers { get; }
}