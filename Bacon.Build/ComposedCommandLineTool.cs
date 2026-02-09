namespace Bacon.Build;

public sealed class ComposedCommandLineTool(ITool<string, Result> tool, string firstArgument, IBuildOutput? buildOutputOverride = null) : ITool<string, Result>
{
    public Result Execute(string arguments, IBuildOutput? buildOutput = null)
    {
        return tool.Execute($"{firstArgument} {arguments}", buildOutput ?? buildOutputOverride);
    }
}