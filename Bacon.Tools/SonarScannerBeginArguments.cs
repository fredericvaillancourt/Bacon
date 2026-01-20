using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} begin {/args:value}")]
public partial class SonarScannerBeginArguments : SonarScannerArguments
{
    [Parameter("key")]
    public string ProjectKey { get; set; }

    [Parameter("name")]
    public string? Name { get; set; }

    [Parameter("version")]
    public string? Version { get; set; }

    [Parameter("d")]
    public string[]? Sonar { get; set; }
}