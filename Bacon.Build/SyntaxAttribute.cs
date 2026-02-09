namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SyntaxAttribute(string syntax) : Attribute
{
    public string Syntax => syntax;
}