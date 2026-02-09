using Bacon.Build;

namespace Bacon.Tools;

public sealed class NoOutputBuildOutput(IBuildOutput buildOutput) : OverrideBuildOutput(buildOutput)
{
    public override void WriteCommandOutput(string line)
    {
        // Nothing
    }
}

public static class BuildOutputExtensions
{
    public static IBuildOutput WithNoOutput(this IBuildOutput buildOutput) => new NoOutputBuildOutput(buildOutput);
}