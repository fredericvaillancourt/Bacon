namespace Bacon.Generator;

[Flags]
internal enum SupportedParamType
{
    String = 1,
    Enum = 2,
    Bool = 4,
    Numeric = 8,
    IsNullable = 16,
    IsArray = 32
}