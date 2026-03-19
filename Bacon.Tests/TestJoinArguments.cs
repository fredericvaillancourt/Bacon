using Bacon.Build;

namespace Bacon.Tests;

[Syntax("join {args}")]
public partial class TestJoinArguments : TestArguments
{
    [Parameter("list-strings", join: ";")]
    public IReadOnlyList<string>? ListStrings { get; }

    [Parameter("list-numerics", join: "+")]
    public IReadOnlyList<int>? ListNumerics { get; }

    [Parameter("list-enums", join: "<->")]
    public IReadOnlyList<TestEnum>? ListEnums { get; }

    [Parameter("dic-strings", join: "@")]
    public IReadOnlyDictionary<string, string>? DicStrings { get; }

    [Parameter("dic-numerics", join: ";")]
    public IReadOnlyDictionary<string, int>? DicNumerics { get; }

    [Parameter("dic-enums", join: " - ")]
    public IReadOnlyDictionary<string, TestEnum>? DicEnums { get; }
}