namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Property)]
public sealed class InputAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}