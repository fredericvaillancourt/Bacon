using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetOutputFormat
{
    [EnumName("table")] Table,
    [EnumName("json")] Json
}