namespace Bacon.Build;

public class OverrideBuildOutput(IBuildOutput buildOutput) : IBuildOutput
{
    protected IBuildOutput BuildOutput { get; } = buildOutput;

    public virtual void BeginTarget(string name)
    {
        BuildOutput.BeginTarget(name);
    }

    public virtual void EndTarget(string name)
    {
        BuildOutput.EndTarget(name);
    }

    public virtual void WriteInformation(string line)
    {
        BuildOutput.WriteInformation(line);
    }

    public virtual void WriteWarning(string line)
    {
        BuildOutput.WriteWarning(line);
    }

    public virtual void WriteError(string line)
    {
        BuildOutput.WriteError(line);
    }

    public virtual void WriteCommandOutput(string line)
    {
        BuildOutput.WriteCommandOutput(line);
    }

    public virtual void WriteCommandError(string line)
    {
        BuildOutput.WriteCommandError(line);
    }

    public virtual void BuildCompleted(TargetResult[] results)
    {
        BuildOutput.BuildCompleted(results);
    }
}