using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} set {args}")]
public partial class DotNetUserSecretsSetArguments : DotNetUserSecretsArguments
{
    public string Name { get; }

    [Parameter(null!, true)]
    public string Value { get; }

    [Parameter("verbose")]
    public bool Verbose { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("file")]
    public string? File { get; }

    [Parameter("configuration")]
    public string? Configuration { get; }

    [Parameter("id", true)]
    public string? Id { get; }
}