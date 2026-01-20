namespace Bacon.Generator;

internal readonly record struct SyntaxToken(SyntaxTokenType Type, char Separator, string? Value);