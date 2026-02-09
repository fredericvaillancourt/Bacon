using Bacon.Build;
using Bacon.Tools;

return await new Build<BuildContext>.Builder()
    .AddTarget("Clean", t => t
        .AddExecutes(static c =>
        {
            c.DotNet.Clean(d => d
                .SetTarget(c.SolutionPath)
                .SetConfiguration(c.Configuration));
            c.OutputDirectory.DeleteDirectory(true);
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
                .SetSolution(c.SolutionPath)
                .AddProperties("Configuration", c.Configuration));
        }), out var buildTarget)
    .AddTarget("ToolRestore", t => t
        .AddExecutes(static c =>
        {
            c.DotNet.ToolRestore();
        }), out var toolRestoreTarget)
    .AddTarget("Inspect", t => t
        .AddAfter(cleanTarget)
        .AddDependsOn(toolRestoreTarget)
        .AddExecutes(c =>
        {
            c.Resharper.Inspect(i => i
                .SetTarget(c.SolutionPath)
                .SetOutput(c.OutputDirectory / "Inspect.json"));
        }))
    .AddTarget("Push", t => t
        .AddDependsOn(buildTarget)
        .AddRequires(static c => c.ApiKey)
        .AddExecutes(c =>
        {
            var version = c.DotNet.MsBuild(d => d
                .AddGetProperty("Version")
                .SetSolution(c.RootDirectory / "Bacon.Build" / "Bacon.Build.csproj")
                .SetBuildOutput(c.BuildOutput.WithNoOutput()))
                .Output.Single(static f => f.Kind == OutputKind.Out).Line;

            c.DotNet.NugetPush(d => d
                .SetApiKey(c.ApiKey)
                .CombineWith([
                    $"Bacon.Build.{version}.nupkg",
                    $"Bacon.Generator.{version}.nupkg",
                    $"Bacon.Tools.{version}.nupkg"
                ], (e, package) => e
                    .SetPackage(c.OutputDirectory / package)
                    .Build()));
        }))
    .SetDefaultTarget(buildTarget)
    .Build()
    .ExecuteAsync(args);

public class BuildContext : Context
{
    public AbsolutePath SolutionPath => RootDirectory / "Bacon.slnx";
    public AbsolutePath OutputDirectory => RootDirectory / "Output";

    [Input(Description = "Build for configuration. Default: Debug")]
    public string Configuration { get; set; } = "Debug";
    [Input(Description = "Nuget api key")]
    public string? ApiKey { get; set; }
}