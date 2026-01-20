namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Class)]
public class ToolAttribute(string Name, ToolLocation Location = ToolLocation.Path, Type? BuildOutput = null) : Attribute;