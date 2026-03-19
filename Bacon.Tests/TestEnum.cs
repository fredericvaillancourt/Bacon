using Bacon.Build;

namespace Bacon.Tests;

[EnumParameter]
public enum TestEnum
{
    [EnumName("value0")] Value0,
    [EnumName("value1")] Value1,
    [EnumName("value2")] Value2,
    [EnumName("value3")] Value3
}