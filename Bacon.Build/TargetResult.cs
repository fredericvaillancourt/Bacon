namespace Bacon.Build;

public sealed record TargetResult(string Name, TargetStatus Status, TimeSpan? Duration);