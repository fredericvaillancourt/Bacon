using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} msbuild {-args:value}")]
public partial class DotNetMsBuildArguments : DotNetArguments
{
    public string Solution { get; }

    [Parameter("target")]
    public IReadOnlyList<string>? Target { get; }

    [Parameter("property")]
    public IReadOnlyDictionary<string, string>? Properties { get; }

    //[Parameter("logger")]
    //public string? Logger { get; }

    //[Parameter("distributedLogger")]
    //public string? DistributedLogger { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    //[Parameter("consoleLoggerParameters")]
    //public string? ConsoleLoggerParameters { get; }

    [Parameter("maxCpuCount")]
    public int? MaxCpuCount { get; }

    //[Parameter("ignoreProjectExtensions")]
    //public string? IgnoreProjectExtensions { get; } //TODO: Also collection

    //[Parameter("toolsVersion")]
    //public string? ToolsVersion { get; }

    //TODO: Missing 1 to 9 extra ...
    //[Parameter("fileLoggerParameters")]
    //public string? FileLoggerParameters { get; }

    //[Parameter("terminalLogger")]
    //public DotNetBuildTerminalLogger? TerminalLogger { get; }

    //[Parameter("terminalLoggerParameters")]
    //public string? TerminalLoggerParameters { get; }

    [Parameter("nodeReuse")]
    public bool? NodeReuse { get; }

    [Parameter("getProperty")]
    public IReadOnlyList<string>? GetProperty { get; }
}

//public enum DotNetBuildTerminalLogger
//{
//    [EnumName("auto")] Auto,
//    [EnumName("on")] On,
//    [EnumName("off")] Off
//}