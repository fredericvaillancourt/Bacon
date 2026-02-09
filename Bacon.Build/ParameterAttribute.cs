namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ParameterAttribute(string argument) : Attribute
{
    public string Argument => argument;
}