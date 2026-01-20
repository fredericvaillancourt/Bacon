namespace Bacon.Build;

[AttributeUsage(AttributeTargets.Class)]
public class SyntaxAttribute(string Syntax) : Attribute;