using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetTestVSTestBlameCrashDumpType
{
    [EnumName("full")] Full,
    [EnumName("mini")] Mini
}