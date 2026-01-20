using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} inspectcode {--args=value}")]
public partial class ResharperInspectArguments : ResharperArguments
{
    [Parameter("format")]
    public string? OutputFormat { get; }

    [Parameter("severity")]
    public ResharperInspectSeverity? Severity { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("build")]
    public bool BuildTarget { get; }

    public string Target { get; }
}