using Bacon.Build;

namespace Bacon.Tools;

[Syntax("format {args}")]
public partial class DotNetFormatArguments : DotNetArguments
{
    public DotNetFormatCommand Command { get; }

    public string Target { get; }

    //TODO: This and other collections could be space separated
    [Parameter("diagnostics", join: " ")]
    public IReadOnlyList<string>? Diagnostics { get; }

    [Parameter("exclude-diagnostics", join: " ")]
    public IReadOnlyList<string>? ExcludeDiagnostics { get; }

    [Parameter("no-restore")]
    public bool NoRestore { get; }

    [Parameter("verify-no-changes")]
    public bool VerifyNoChanges { get; }

    [Parameter("include", join: " ")]
    public IReadOnlyList<string>? Include { get; }

    [Parameter("exclude", join: " ")]
    public IReadOnlyList<string>? Exclude { get; }

    [Parameter("include-generated")]
    public bool IncludeGenerated { get; }

    [Parameter("verbosity")]
    public DotNetVerbosity? Verbosity { get; }

    [Parameter("binarylog")]
    public string? BinaryLog { get; }

    [Parameter("report")]
    public string? Report { get; }
}