namespace Bacon.Build;

public interface IConfigurationSource<in T> where T : class
{
    Task ApplyAsync(T context, IReadOnlyList<InputInfo> inputsInfo, BuildConfiguration buildConfiguration);
}