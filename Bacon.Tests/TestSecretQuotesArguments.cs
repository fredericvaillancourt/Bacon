using Bacon.Build;

namespace Bacon.Tests;

[Syntax("secret {args=\"value\"}")]
public partial class TestSecretQuotesArguments : TestArguments
{
    //TODO: Cannot have required and secret now ...
    [Parameter("secret0", true)]
    public IReadOnlyList<string>? Secret0 { get; }

    [Parameter("param0")]
    public string? Param0 { get; }

    [Parameter("secret1", true)]
    public int? Secret1 { get; }

    [Parameter("param1")]
    public string? Param1 { get; }

    [Parameter("secret2", true)]
    public bool? Secret2 { get; }

    [Parameter("param2")]
    public string? Param2 { get; }
}