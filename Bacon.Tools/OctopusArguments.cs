using Bacon.Build;

namespace Bacon.Tools;

[Tool("Octopus.DotNet.Cli", ToolLocation.Tool)]
[Syntax("{args}")]
public abstract partial class OctopusArguments : Arguments
{
    [Parameter("logLevel")]
    public OctopusLogLevel? LogLevel { get; }

    [Parameter("outputFormat")]
    public OctopusOutputFormat? OutputFormat { get; }
}