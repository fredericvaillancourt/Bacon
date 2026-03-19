using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} list {args}")]
public partial class DotNetNewListArguments : DotNetNewBaseArguments
{
    public string TemplateName { get; }

    [Parameter("author")]
    public IReadOnlyList<string>? Authors { get; }

    [Parameter("language")]
    public IReadOnlyList<string>? Languages { get; }

    [Parameter("type")]
    public IReadOnlyList<string>? Types { get; }

    [Parameter("tag")]
    public IReadOnlyList<string>? Tags { get; }

    [Parameter("ignore-constraints")]
    public bool IgnoreConstraints { get; }

    [Parameter("output")]
    public string? Output { get; }

    [Parameter("project")]
    public string? Project { get; }

    [Parameter("columns-all")]
    public bool ColumnsAll { get; }

    [Parameter("columns")]
    public IReadOnlyList<DotNetNewSearchColumns>? Columns { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; } //TODO: Help does not list detailed ...?

    [Parameter("diagnostics")]
    public bool Diagnostics { get; }
}