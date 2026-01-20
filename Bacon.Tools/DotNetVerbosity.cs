using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetVerbosity
{
    [EnumName("q")] Quiet,
    [EnumName("m")] Minimal,
    [EnumName("n")] Normal,
    [EnumName("d")] Detailed,
    [EnumName("diag")] Diagnostic
}