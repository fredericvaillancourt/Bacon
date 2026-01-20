using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum ResharperInspectSeverity
{
    [EnumName("INFO")] Info,
    [EnumName("HINT")] Hint,
    [EnumName("SUGGESTION")] Suggestion,
    [EnumName("WARNING")] Warning,
    [EnumName("ERROR")] Error
}