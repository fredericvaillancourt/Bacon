using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetTestVSTestBlameHangDumpType
{
    [EnumName("full")] Full,
    [EnumName("mini")] Mini,
    [EnumName("none")] None
}