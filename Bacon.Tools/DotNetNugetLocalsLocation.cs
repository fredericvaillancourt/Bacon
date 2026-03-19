using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetNugetLocalsLocation
{
    [EnumName("all")] All,
    [EnumName("http-cache")] HttpCache,
    [EnumName("global-packages")] GlobalPackages,
    [EnumName("temp")] Temp
}