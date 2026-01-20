namespace Bacon.Build;

public class GitHubBuildOutput(TimeProvider? timeProvider = null) : ConsoleBuildOutput(timeProvider)
{
    public new static GitHubBuildOutput Instance { get; } = new();

    public override void BeginTarget(string name)
    {
        Console.WriteLine($"::group::{name}");
    }

    public override void EndTarget(string _)
    {
        Console.WriteLine("::endgroup::");
    }
}