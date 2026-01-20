namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Property)]
public class ParameterAttribute(string Argument) : Attribute;