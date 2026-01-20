using Bacon.Build;
using Bacon.Tools;

return await new Build<YourContext>.Builder()
    .AddTarget("Clean", t => t
        .AddExecutes(static c =>
        {
            c.DotNet.Clean(d => d
                .SetTarget(c.SolutionPath)
                .SetConfiguration("Debug"));
        }), out var cleanTarget)
    .AddTarget("Restore", t => t
        .AddAfter(cleanTarget)
        .AddExecutes(static c =>
        {
            c.DotNet.Restore(d => d
                .SetTarget(c.SolutionPath));
        }), out var restoreTarget)
    .AddTarget("Build", t => t
        .AddDependsOn(restoreTarget)
        .AddExecutes(c =>
        {
            c.DotNet.MsBuild(d => d
                .SetSolution(c.SolutionPath));
        }), out var buildTarget)
    .SetDefaultTarget(buildTarget)
    .Context(static c =>
    {
        c.SolutionPath = c.RootDirectory / "YourProject.sln";
    })
    .Build()
    .ExecuteAsync(args);

internal class YourContext : Context
{
    public AbsolutePath SolutionPath { get; set; }
}