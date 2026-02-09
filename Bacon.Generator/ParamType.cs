namespace Bacon.Generator;

internal readonly record struct ParamType(string Name, SupportedParamType Type)
{
    public bool IsBaseReferenceType => (Type & SupportedParamType.String) != 0;
    public bool IsBaseValueType => !IsBaseReferenceType;

    public bool IsReferenceType => (Type & (SupportedParamType.String | SupportedParamType.IsList | SupportedParamType.IsDictionary)) != 0;
    public bool IsValueType => !IsReferenceType;

    public bool IsNullable => (Type & SupportedParamType.IsNullable) != 0;
    public bool IsBool => (Type & SupportedParamType.Bool) != 0;
    public bool IsList => (Type & SupportedParamType.IsList) != 0;
    public bool IsDictionary => (Type & SupportedParamType.IsDictionary) != 0;
    public bool IsCollection => (Type & (SupportedParamType.IsList | SupportedParamType.IsDictionary)) != 0;
    public bool IsEnum => (Type & SupportedParamType.Enum) != 0;
    public bool IsString => (Type & SupportedParamType.String) != 0;

    public ParamType AsNullable() => IsNullable ? this : new ParamType(Name, Type | SupportedParamType.IsNullable);

    public override string ToString()
    {
        if ((Type & SupportedParamType.IsList) != 0)
        {
            return (Type & SupportedParamType.IsNullable) != 0 ? $"IReadOnlyList<{Name}>?" : $"IReadOnlyList<{Name}>";
        }

        return (Type & SupportedParamType.IsNullable) != 0 ? $"{Name}?" : Name;
    }

    public static implicit operator string(ParamType paramType)
    {
        return paramType.ToString();
    }
}