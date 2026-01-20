using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum OctopusPushOverwriteMode
{
    FailIfExists,
    OverwriteExisting,
    IgnoreIfExists
}