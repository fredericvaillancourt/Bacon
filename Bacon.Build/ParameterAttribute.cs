namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ParameterAttribute(string argument, bool isSecret = false, string? join = null) : Attribute
{
    public string Argument => argument;
    public bool IsSecret => isSecret;
    public string? Join => join;
}