namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Property)]
public class FromCommandLineAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}