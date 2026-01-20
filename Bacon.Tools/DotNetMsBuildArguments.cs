using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} msbuild {-args:value}")]
public partial class DotNetMsBuildArguments : DotNetArguments
{
    public string Solution { get; }

    [Parameter("target")]
    public string? Target { get; } //TODO: This is a collection which is separated by ; when --target, but use -t multiple time ...

    [Parameter("property")]
    public string? Properties { get; } //TODO: This is a collection which is separated by ; when --property, but use -p multiple time ... Also is a key=value ...

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
}

//public enum DotNetBuildTerminalLogger
//{
//    [EnumName("auto")] Auto,
//    [EnumName("on")] On,
//    [EnumName("off")] Off
//}