using Bacon.Build;

namespace Bacon.Tools;

[Syntax("user-jwts clear {args}")]
public partial class DotNetUserJwtsClearArguments : DotNetArguments
{
    [Parameter("project")]
    public string? Project { get; }

    [Parameter("output")]
    public DotNetUserJwtsOutput? Output { get; }

    [Parameter("force")]
    public bool Force { get; }

    [Parameter("appsettings-file")]
    public string? AppSettingsFile { get; }
}