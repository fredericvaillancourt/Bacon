using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} update {args}")]
public partial class DotNetNewUpdateArguments : DotNetNewBaseArguments
{
    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("add-source")]
    public string? AddSource { get; }

    [Parameter("dry-run")]
    public bool DryRun { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; } //TODO: Help does not list detailed ...?

    [Parameter("diagnostics")]
    public bool Diagnostics { get; }
}