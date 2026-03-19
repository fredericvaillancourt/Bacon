using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetUserJwtsOutput
{
    [EnumName("default")] Default,
    [EnumName("token")] Token,
    [EnumName("json")] Json
}