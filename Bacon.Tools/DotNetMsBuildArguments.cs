using Bacon.Build;

namespace Bacon.Tools;

[Syntax("msbuild {-args:value}")]
public partial class DotNetMsBuildArguments : DotNetArguments
{
    public string Solution { get; }

    [Parameter("target", join: ";")]
    public IReadOnlyList<string>? Target { get; }

    [Parameter("property", join: ";")]
    public IReadOnlyDictionary<string, string>? Properties { get; }

    [Parameter("logger")]
    public string? Logger { get; }

    [Parameter("distributedLogger")]
    public string? DistributedLogger { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("consoleLoggerParameters")]
    public string? ConsoleLoggerParameters { get; }

    [Parameter("maxCpuCount")]
    public int? MaxCpuCount { get; }

    [Parameter("ignoreProjectExtensions", join: ";")]
    public IReadOnlyList<string>? IgnoreProjectExtensions { get; }

    [Parameter("toolsVersion")]
    public string? ToolsVersion { get; }

    //TODO: Missing 1 to 9 extra ...
    //[Parameter("fileLoggerParameters")]
    //public string? FileLoggerParameters { get; }

    [Parameter("terminalLogger")]
    public DotNetBuildTerminalLogger? TerminalLogger { get; }

    [Parameter("terminalLoggerParameters")]
    public string? TerminalLoggerParameters { get; }

    [Parameter("nodeReuse")]
    public bool? NodeReuse { get; }

    [Parameter("preprocess")]
    public string? Preprocess { get; }

    [Parameter("targets")]
    public string? Targets { get; }

    [Parameter("warnAsError", join: ";")]
    public IReadOnlyList<string>? WarnAsError { get; }

    [Parameter("warnAsMessage", join: ";")]
    public IReadOnlyList<string>? WarnAsMessage { get; }

    [Parameter("binaryLogger")]
    public string? BinaryLogger { get; }

    [Parameter("check")]
    public bool Check { get; }

    [Parameter("restore")]
    public bool Restore { get; }

    [Parameter("profileEvaluation")]
    public string? ProfileEvaluation { get; }

    [Parameter("restoreProperty", join: ";")]
    public IReadOnlyDictionary<string, string>? RestoreProperty { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("isolateProjects")]
    public DotNetBuildIsolateProjects? IsolateProjects { get; }

    [Parameter("graphBuild")]
    public bool GraphBuild { get; }

    [Parameter("inputResultsCaches", join: ";")]
    public IReadOnlyList<string>? InputResultsCaches { get; }

    [Parameter("outputResultsCache")]
    public string? OutputResultsCache { get; }

    [Parameter("lowPriority")]
    public bool LowPriority { get; }

    [Parameter("question")]
    public bool Question { get; }

    [Parameter("detailedSummary")]
    public bool DetailedSummary { get; }

    [Parameter("getProperty", join: ";")]
    public IReadOnlyList<string>? GetProperty { get; }

    [Parameter("getItem", join: ";")]
    public IReadOnlyList<string>? GetItem { get; }

    [Parameter("getTargetResult", join: ";")]
    public IReadOnlyList<string>? GetTargetResult { get; }

    [Parameter("getResultOutputFile")]
    public string? GetResultOutputFile { get; }

    [Parameter("featureAvailability", join: ";")]
    public IReadOnlyList<string>? FeatureAvailability { get; }

    [Parameter("multithreaded")]
    public bool Multithreaded { get; }

    [Parameter("version")]
    public bool Version { get; }

    [Parameter("noLogo")]
    public bool NoLogo { get; }

    [Parameter("noAutoResponse")]
    public bool NoAutoResponse { get; }

    [Parameter("noConsoleLogger")]
    public bool NoConsoleLogger { get; }

    //TODO: fileLogger[n]

    [Parameter("distributedFileLogger")]
    public bool DistributedFileLogger { get; }

    //TODO: @file
}

[EnumParameter]
public enum DotNetBuildTerminalLogger
{
    [EnumName("auto")] Auto,
    [EnumName("on")] On,
    [EnumName("off")] Off
}

[EnumParameter]
public enum DotNetBuildIsolateProjects
{
    True,
    MessageUponIsolationViolation,
    False
}