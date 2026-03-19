using Bacon.Build;

namespace Bacon.Tools;

[Syntax("dev-certs https {args}")]
public partial class DotNetDevCertsHttpsArguments : DotNetArguments
{
    [Parameter("export-path")]
    public string? ExportPath { get; }

    [Parameter("password", true)]
    public string? Password { get; }

    [Parameter("no-password")]
    public bool NoPassword { get; }

    [Parameter("check")]
    public bool Check { get; }

    [Parameter("clean")]
    public bool Clean { get; }

    [Parameter("import")]
    public string? Import { get; }

    [Parameter("format")]
    public DotNetDevCertsHttpsFormat? Format { get; }

    [Parameter("trust")]
    public bool Trust { get; }

    [Parameter("verbose")]
    public bool Verbose { get; }

    [Parameter("quiet")]
    public bool Quiet { get; }

    [Parameter("check-trust-machine-readable")]
    public bool CheckTrustMachineReadable { get; }
}