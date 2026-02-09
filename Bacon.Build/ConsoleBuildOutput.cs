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
        int maxNameLength = results.Max(static n => n.Name.Length);
        string[] durations = results.Select(static r => r.Duration?.ToShortString() ?? string.Empty).ToArray();
        int maxDurationLength = durations.Max(static n => n.Length);
        int boxInternalWidth = maxNameLength + maxDurationLength + 5;
        Console.WriteLine($"╔{new string('═', boxInternalWidth)}╗");
        Console.WriteLine($"║ Results{new string(' ', boxInternalWidth - 8)}║");
        Console.WriteLine($"╟─{new string('─', maxNameLength)}─┬─{new string('─', maxDurationLength)}─╢");

        for (int index = 0; index < results.Length; index++)
        {
            TargetResult result = results[index];
            string duration = durations[index];

            string suffix = "";
            if (TargetResultColors.TryGetValue(result.Status, out var prefix))
            {
                suffix = "\e[0m";
            }
            else
            {
                prefix = "";
            }

            Console.WriteLine($"║ {prefix}{result.Name}{suffix}{new string(' ', maxNameLength - result.Name.Length)} │ {duration}{new string(' ', maxDurationLength - duration.Length)} ║");
        }

        Console.WriteLine($"╚═{new string('═', maxNameLength)}═╧═{new string('═', maxDurationLength)}═╝");
    }

    private void WriteCommand(string tag, string line)
    {
        Console.WriteLine($"{_timeProvider.GetLocalNow():T} [{tag}] {line}");
    }
}