using Bacon.Build;

namespace Bacon.Tools;

[Syntax("test {args}")]
public partial class DotNetTestMTPArguments : DotNetArguments
{
    [Parameter("project")]
    public string? Project { get; }

    [Parameter("solution")]
    public string? Solution { get; }

    [Parameter("test-modules")]
    public string? TestModules { get; }

    [Parameter("root-directory")]
    public string? RootDirectory { get; }

    [Parameter("results-directory")]
    public string? ResultsDirectory { get; }

    [Parameter("config-file")]
    public string? ConfigFile { get; }

    [Parameter("diagnostic-output-directory")]
    public string? DiagnosticOutputDirectory { get; }

    [Parameter("max-parallel-test-modules")]
    public int? MaxParallelTestModules { get; }

    [Parameter("minimum-expected-tests")]
    public int? MinimumExpectedTests { get; }

    [Parameter("arch")]
    public string? Architecture { get; }

    [Parameter("environment")]
    public IReadOnlyDictionary<string, string>? Environments { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("framework")]
    public string? Framework { get; }

    [Parameter("os")]
    public string? OperatingSystem { get; }

    [Parameter("runtime")]
    public string? Runtime { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("no-build")]
    public bool NoBuild { get; }

    [Parameter("no-ansi")]
    public bool NoAnsi { get; }

    [Parameter("no-build")]
    public bool NoProgress { get; }

    [Parameter("output")]
    public DotNetTestMTPOutput Output { get; }

    [Parameter("list-tests")]
    public bool ListTests { get; }

    [Parameter("no-launch-profile")]
    public bool NoLaunchProfile { get; }

    [Parameter("no-launch-profile-arguments")]
    public bool NoLaunchProfileArguments { get; }
}