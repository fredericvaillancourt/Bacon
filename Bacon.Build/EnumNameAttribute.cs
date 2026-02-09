namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumNameAttribute(string name) : Attribute
{
    public string Name => name;
}