using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} clear {args}")]
public partial class DotNetUserSecretsClearArguments : DotNetUserSecretsArguments
{
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