using Bacon.Build;

namespace Bacon.Tools;

[Tool("gitversion.tool", ToolLocation.Tool, typeof(NoOutputBuildOutput))]
[Syntax("{args}")]
public partial class GitVersionArguments : Arguments
{
    public string? Path { get; }

    //TODO: There are more arguments, but we do not use them yet.
    //TODO: This tool uses / and does not support -- ... we need to add it to syntax and generate accordingly.
}