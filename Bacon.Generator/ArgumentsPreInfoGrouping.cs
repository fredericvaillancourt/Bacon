namespace Bacon.Generator;

internal class ArgumentsPreInfoGrouping(ArgumentsPreInfo argumentsPreInfo)
{
    private ClassInfo? _classInfo;
    private EquatableArray<Parameter>? _baseParameters;

    public ArgumentsPreInfo ArgumentsPreInfo => argumentsPreInfo;

    public EquatableArray<Parameter> BaseParameters
    {
        get
        {
            if (!_baseParameters.HasValue)
            {
                int count = 0;
                var parent = Base;
                while (parent != null)
                {
                    count += parent.ArgumentsPreInfo.Parameters.Length;
                    parent = parent.Base;
                }

                var parameters = new Parameter[count];
                count = 0;
                parent = Base;
                while (parent != null)
                {
                    var p = parent.ArgumentsPreInfo.Parameters;
                    for (int i = 0; i < p.Length; ++i)
                    {
                        parameters[count++] = p[i];
                    }

                    parent = parent.Base;
                }

                _baseParameters = new(parameters);
            }

            return _baseParameters.Value;
        }
    }

    public bool HasRequiredParameters => ArgumentsPreInfo.Parameters.Any(static p => !p.Type.IsNullable) || Base is { HasRequiredParameters: true };

    // ClassName
    // IsAbstract
    public ArgumentsPreInfoGrouping? Base { get; set; }

    // Only for the group by so no
    public ArgumentsPreInfoGrouping Top => Base ?? this;

    // Important
    public bool IsRoot => Base == null;

    public ClassInfo ClassInfo
    {
        get
        {
            if (_classInfo == null)
            {
                _classInfo = ClassInfo.From(ArgumentsPreInfo);
            }

            return _classInfo;
        }
    }
}