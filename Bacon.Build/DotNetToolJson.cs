namespace Bacon.Build;

public class DotNetToolsJson
{
    public int Version { get; set; }
    public bool IsRoot { get; set; }
    public Dictionary<string, DotNetToolsTool> Tools { get; set; }
}

public class DotNetToolsTool
{
    public string Version { get; set; }
    public string[] Commands { get; set; }
    public bool RollForward { get; set; }
}