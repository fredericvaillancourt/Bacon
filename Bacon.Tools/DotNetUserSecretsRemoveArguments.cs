using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} remove {args}")]
public partial class DotNetUserSecretsRemoveArguments : DotNetUserSecretsArguments
{
    public string Name { get; }

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