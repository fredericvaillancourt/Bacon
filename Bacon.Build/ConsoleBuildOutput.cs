namespace Bacon.Build;

public class ConsoleBuildOutput : IBuildOutput
{
    private const string StdOutPrefix = "\e[34mStdOut\e[0m";
    private const string StdErrPrefix = "\e[31mStdErr\e[0m";

    private static readonly Dictionary<TargetStatus, string> TargetResultColors = new()
    {
        { TargetStatus.Succeeded, "\e[32m" },
        { TargetStatus.Skipped, "\e[90m" },
        { TargetStatus.Failed, "\e[31m" }
    };

    private readonly TimeProvider _timeProvider;

    public static ConsoleBuildOutput Instance { get; } = new();

    protected ConsoleBuildOutput(TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public virtual void BeginTarget(string name)
    {
        string line = new('═', name.Length + 2);
        Console.WriteLine($"╔{line}╗");
        Console.WriteLine($"║ {name} ║");
        Console.WriteLine($"╚{line}╝");
    }

    public virtual void EndTarget(string _)
    {
    }

    public void WriteInformation(string line)
    {
        Console.WriteLine(line);
    }

    public void WriteWarning(string line)
    {
        Console.WriteLine($"\e[93m{line}\e[0m");
    }

    public void WriteError(string line)
    {
        Console.WriteLine($"\e[31m{line}\e[0m");
    }

    public void WriteCommandOutput(string line)
    {
        WriteCommand(StdOutPrefix, line);
    }

    public void WriteCommandError(string line)
    {
        WriteCommand(StdErrPrefix, line);
    }

    public void BuildCompleted(TargetResult[] results)
    {
        Console.WriteLine();
        Console.WriteLine("Results");

        int maxLength = results.Max(static n => n.Name.Length);

        foreach (var result in results)
        {
            string suffix = "";
            if (TargetResultColors.TryGetValue(result.Status, out var prefix))
            {
                suffix = "\e[0m";
            }
            else
            {
                prefix = "";
            }

            if (result.Duration.HasValue)
            {
                Console.WriteLine($"{prefix}{result.Name}{suffix}{new string(' ', maxLength - result.Name.Length)} {result.Duration.Value.ToShortString()}");
            }
            else
            {
                Console.WriteLine($"{prefix}{result.Name}{suffix}");
            }
        }
    }

    private void WriteCommand(string tag, string line)
    {
        Console.WriteLine($"{_timeProvider.GetLocalNow():T} [{tag}] {line}");
    }
}