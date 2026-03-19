using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetNewSearchColumns
{
    [EnumName("author")] Author,
    [EnumName("language")] Language,
    [EnumName("tags")] Tags,
    [EnumName("type")] Type
}