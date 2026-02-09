namespace Bacon.Generator;

[Flags]
internal enum SyntaxQuoteStyle : byte
{
    Automatic, // Automatic around value
    Value, // Forced around value
    Whole // Force around whole option
}