using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} details {args}")]
public partial class DotNetNewDetailsArguments : DotNetNewBaseArguments
{
    public string PackageIdentifier { get; }

    [Parameter("interactive")]
    public bool Interactive { get; }

    [Parameter("add-source")]
    public string? AddSource { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; } //TODO: Help does not list detailed ...?

    [Parameter("diagnostics")]
    public bool Diagnostics { get; }
}