namespace Bacon.Build;

public interface IBuildOutput
{
    void BeginTarget(string name);
    void EndTarget(string name);
    void WriteInformation(string line);
    void WriteWarning(string line);
    void WriteError(string line);
    void WriteCommandOutput(string line);
    void WriteCommandError(string line);
    void BuildCompleted(TargetResult[] results);
}