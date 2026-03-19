namespace Bacon.Generator;

[Flags]
internal enum SupportedParamType
{
    String = 1,
    Enum = 2,
    Bool = 4,
    Char = 8,
    Numeric = 16,
    IsNullable = 32,
    IsList = 64,
    IsDictionary = 128
}