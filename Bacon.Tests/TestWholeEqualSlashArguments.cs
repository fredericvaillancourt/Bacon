using Bacon.Build;

namespace Bacon.Tests;

[Syntax("wes {\"/args=value\"}")]
public partial class TestWholeEqualSlashArguments : TestArguments
{
    [Parameter("string")]
    public string? String { get; }

    [Parameter("numeric")]
    public int? Numeric { get; }

    [Parameter("enum")]
    public TestEnum? Enum { get; }

    [Parameter("list-strings")]
    public IReadOnlyList<string>? ListStrings { get; }

    [Parameter("list-numerics")]
    public IReadOnlyList<int>? ListNumerics { get; }

    [Parameter("list-enums")]
    public IReadOnlyList<TestEnum>? ListEnums { get; }

    [Parameter("dic-strings")]
    public IReadOnlyDictionary<string, string>? DicStrings { get; }

    [Parameter("dic-numerics")]
    public IReadOnlyDictionary<string, int>? DicNumerics { get; }

    [Parameter("dic-enums")]
    public IReadOnlyDictionary<string, TestEnum>? DicEnums { get; }
}