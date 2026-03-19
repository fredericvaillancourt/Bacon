using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} uninstall {args}")]
public partial class DotNetNewUnInstallArguments : DotNetNewBaseArguments
{
    public string Package { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; } //TODO: Help does not list detailed ...?

    [Parameter("diagnostics")]
    public bool Diagnostics { get; }
}