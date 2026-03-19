using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetWorkloadSearchFormat
{
    [EnumName("json")] Json,
    [EnumName("list")] List
}