using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} install {args}")]
public partial class DotNetNewInstallArguments : DotNetNewBaseArguments
{
    public string Package { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("add-source")]
    public string? AddSource { get; }

    [Parameter("force")]
    public bool Force { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; } //TODO: Help does not list detailed ...?

    [Parameter("diagnostics")]
    public bool Diagnostics { get; }
}