using Bacon.Build;

namespace Bacon.Tools;

[Syntax("user-jwts remove {args}")]
public partial class DotNetUserJwtsRemoveArguments : DotNetArguments
{
    public string Id { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("output")]
    public DotNetUserJwtsOutput? Output { get; }

    [Parameter("appsettings-file")]
    public string? AppSettingsFile { get; }
}