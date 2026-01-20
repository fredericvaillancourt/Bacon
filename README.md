# Bacon Build
## About
Bacon Build is a library to automate building of source code using .Net. It is still a very early version so expect breaking changes.

## How To Use
Here is a very basic .Net build. You can run it using `dotnet --project TheProject.csproj`.
```c#
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
```