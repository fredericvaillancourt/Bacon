using Bacon.Build;

namespace Bacon.Tests;

[Syntax("col {args}")]
public partial class TestCollectionArguments : TestArguments
{
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