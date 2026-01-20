using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} -- {args}")]
public partial class DotNetRunTestsArguments : DotNetRunArguments
{
    [Parameter("report-trx")]
    public bool ReportTrx { get; }

    [Parameter("report-trx-filename")]
    public string? ReportTrxFilename { get; }

    [Parameter("coverage")]
    public bool Coverage { get; }

    [Parameter("coverage-output-format")]
    public MsTestCoverageFormat? CoverageOutputFormat { get; }

    [Parameter("results-directory")]
    public string? ResultsDirectory { get; }
}