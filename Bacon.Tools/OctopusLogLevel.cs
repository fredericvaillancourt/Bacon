using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum OctopusLogLevel
{
    [EnumName("verbose")] Verbose,
    [EnumName("debug")] Debug,
    [EnumName("information")] Information,
    [EnumName("warning")] Warning,
    [EnumName("error")] Error,
    [EnumName("fatal")] Fatal
}