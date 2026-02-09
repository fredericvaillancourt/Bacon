namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ToolAttribute(string name, ToolLocation location = ToolLocation.Path, Type? buildOutput = null) : Attribute
{
    public string Name => name;
    public ToolLocation Location => location;
    public Type? BuildOutput => buildOutput;
}