using Bacon.Build;

namespace Bacon.Tools;

[Tool("dotnet-sonarscanner", ToolLocation.Tool)]
[Syntax("{/args:value}")]
public abstract partial class SonarScannerArguments : Arguments
{
}