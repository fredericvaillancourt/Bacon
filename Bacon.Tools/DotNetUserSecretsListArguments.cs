using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list {args}")]
public partial class DotNetUserSecretsListArguments : DotNetUserSecretsArguments
{
    [Parameter("json")]
    public bool Json { get; }

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