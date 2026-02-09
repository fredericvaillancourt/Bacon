using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} end {/args:value}")]
public partial class SonarScannerEndArguments : SonarScannerArguments
{
    [Parameter("d")]
    public IReadOnlyDictionary<string, string>? Defines { get; }
}