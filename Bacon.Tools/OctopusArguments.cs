using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{args}")]
[Tool("Octopus.DotNet.Cli", ToolLocation.Tool)]
public abstract partial class OctopusArguments : Arguments
{
    [Parameter("logLevel")]
    public OctopusLogLevel? LogLevel { get; }

    [Parameter("outputFormat")]
    public OctopusOutputFormat? OutputFormat { get; }
}