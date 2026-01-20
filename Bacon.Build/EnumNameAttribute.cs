namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Field)]
public class EnumNameAttribute(string Name) : Attribute;