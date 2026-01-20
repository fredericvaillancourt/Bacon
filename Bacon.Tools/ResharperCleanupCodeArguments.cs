using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} cleanupCode {--args=value}")]
public partial class ResharperCleanupCodeArguments : ResharperArguments
{
    public string Target { get; }
}