namespace Bacon.Generator;

internal readonly record struct SyntaxToken(SyntaxTokenType Type, SyntaxQuoteStyle QuoteStyle, char Separator, string? Value);