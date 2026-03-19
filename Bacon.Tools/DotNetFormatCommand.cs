using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetFormatCommand
{
    [EnumName("whitespace")] Whitespace,
    [EnumName("style")] Style,
    [EnumName("analyzers")] Analyzers
}