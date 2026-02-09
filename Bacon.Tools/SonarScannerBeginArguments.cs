using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} begin {/args:value}")]
public partial class SonarScannerBeginArguments : SonarScannerArguments
{
    [Parameter("key")]
    public string ProjectKey { get; }

    [Parameter("name")]
    public string? Name { get; }

    [Parameter("version")]
    public string? Version { get; }

    [Parameter("d")]
    public IReadOnlyDictionary<string, string>? Defines { get; }
}