using System.Diagnostics;
using System.Text;

namespace Bacon.Build;

public sealed class CommandLineTool(string fileName, IBuildOutput defaultBuildOutput, string? prefix = null) : ITool<string, Result>
{
    public string FileName => fileName;
    public IBuildOutput DefaultBuildOutput => defaultBuildOutput;
    public string? Prefix => prefix;

    public Result Execute(ref ArgumentsStringHandler handler, IBuildOutput? overrideBuildOutput = null)
    {
        string redacted = handler.GetRedactedString();

        if (prefix == null)
        {
            return Execute(handler.GetSecretString() ?? redacted, redacted, overrideBuildOutput);
        }

        string? secret = handler.GetSecretString();
        redacted = $"{prefix} {redacted}";
        secret = secret == null ? redacted : $"{prefix} {secret}";
        return Execute(secret, redacted, overrideBuildOutput ?? overrideBuildOutput);
    }

    public Result Execute(string arguments, IBuildOutput? overrideBuildOutput = null)
    {
        return Execute(arguments, arguments, overrideBuildOutput);
    }

    private Result Execute(string argumentsToExecute, string argumentsToLog, IBuildOutput? overrideBuildOutput = null)
    {
        var buildOutput = overrideBuildOutput ?? defaultBuildOutput;

        buildOutput.WriteInformation($"[Command] {fileName} {argumentsToLog}");

        var process = Process.Start(new ProcessStartInfo(fileName, argumentsToExecute)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        }) ?? throw new InvalidOperationException("Could not start");

        if (process == null)
        {
            throw new FileNotFoundException("Could not find: " + fileName);
        }

        var output = new List<Output>();
        var lck = new Lock();

        process.OutputDataReceived += (_, args) =>
        {
            if (args.Data == null)
            {
                return;
            }

            lock (lck)
            {
                output.Add(new Output(OutputKind.Out, args.Data));
                buildOutput.WriteCommandOutput(args.Data);
            }
        };

        process.ErrorDataReceived += (_, args) =>
        {
            if (args.Data == null)
            {
                return;
            }

            lock (lck)
            {
                output.Add(new Output(OutputKind.Err, args.Data));
                buildOutput.WriteCommandError(args.Data);
            }
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        return process.ExitCode == 0 ?
            new Result(output) :
            throw new InvalidOperationException($"Exit code {process.ExitCode} when executing {fileName} {argumentsToLog}");
    }
}