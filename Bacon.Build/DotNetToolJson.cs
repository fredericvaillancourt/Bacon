namespace Bacon.Build;

public sealed class DotNetToolsJson
{
    public int Version { get; set; }
    public bool IsRoot { get; set; }
    public Dictionary<string, DotNetToolsTool>? Tools { get; set; }
}

public sealed class DotNetToolsTool
{
    public string? Version { get; set; }
    public string[]? Commands { get; set; }
    public bool RollForward { get; set; }
}