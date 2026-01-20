using Bacon.Build;

namespace Bacon.Tools;

[Tool("gitversion.tool", ToolLocation.Tool, typeof(GitVersionBuildOutput))]
[Syntax("{args}")]
public partial class GitVersionArguments : Arguments
{
    public string? Path { get; set; }

    //TODO: There are more arguments, but we do not use them yet.
    //TODO: This tool uses / and does not support -- ... we need to add it to syntax and generate accordingly.
}

internal class GitVersionBuildOutput(IBuildOutput buildOutput) : OverrideBuildOutput(buildOutput)
{
    public override void WriteCommandOutput(string line)
    {
        // Nothing
    }
}