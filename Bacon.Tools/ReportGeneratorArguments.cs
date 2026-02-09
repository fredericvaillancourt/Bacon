using Bacon.Build;

namespace Bacon.Tools;

[Tool("dotnet-reportgenerator-globaltool", ToolLocation.Tool)]
[Syntax("{\"-args:value\"}")]
public partial class ReportGeneratorArguments : Arguments
{
    [Parameter("reports")]
    public string? Reports { get; }

    [Parameter("targetdir")]
    public string? TargetDirectory { get; }

    [Parameter("reporttypes")]
    public string? ReportTypes { get; }
}