namespace Bacon.Build;

public record TargetResult(string Name, TargetStatus Status, TimeSpan? Duration);