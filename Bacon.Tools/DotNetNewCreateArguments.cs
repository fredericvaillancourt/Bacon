using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} create {args}")]
public partial class DotNetNewCreateArguments : DotNetNewBaseArguments
{
    public string TemplateShortName { get; }

    public IReadOnlyList<string>? TemplateArguments { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("name")]
    public string? Name { get; }

    [Parameter("dry-run")]
    public bool DryRun { get; }

    [Parameter("force")]
    public bool Force { get; }

    [Parameter("no-update-check")]
    public bool NoUpdateCheck { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; } //TODO: Help does not list detailed ...?

    [Parameter("diagnostics")]
    public bool Diagnostics { get; }
}