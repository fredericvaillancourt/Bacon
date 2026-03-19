using Bacon.Build;

namespace Bacon.Tests;

[Syntax("bool {args}")]
public partial class TestBooleanArguments : TestArguments
{
    [Parameter("optional0")]
    public bool Optional0 { get; }

    [Parameter("optional1")]
    public bool Optional1 { get; }

    [Parameter("nullable-false")]
    public bool? NullableFalse { get; }

    [Parameter("nullable-true")]
    public bool? NullableTrue { get; }

    [Parameter("nullable-null")]
    public bool? NullableNull { get; }
}