namespace Bacon.Generator;

internal readonly record struct ParamType(string Name, SupportedParamType Type)
{
    public bool IsBaseReferenceType => (Type & SupportedParamType.String) != 0;
    public bool IsBaseValueType => !IsBaseReferenceType;

    public bool IsReferenceType => (Type & (SupportedParamType.String | SupportedParamType.IsArray)) != 0;
    public bool IsValueType => !IsReferenceType;

    public bool IsNullable => (Type & SupportedParamType.IsNullable) != 0;
    public bool IsBool => (Type & SupportedParamType.Bool) != 0;
    public bool IsArray => (Type & SupportedParamType.IsArray) != 0;
    public bool IsEnum => (Type & SupportedParamType.Enum) != 0;
    public bool IsString => (Type & SupportedParamType.String) != 0;

    public ParamType AsNullable() => IsNullable ? this : new ParamType(Name, Type | SupportedParamType.IsNullable);

    public override string ToString()
    {
        if ((Type & SupportedParamType.IsArray) != 0)
        {
            return (Type & SupportedParamType.IsNullable) != 0 ? $"{Name}[]?" : $"{Name}[]";
        }

        return (Type & SupportedParamType.IsNullable) != 0 ? $"{Name}?" : Name;
    }

    public static implicit operator string(ParamType paramType)
    {
        return paramType.ToString();
    }
}