using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Bacon.Build;

public class Context
{
    private readonly Dictionary<object, object> _context = new();

    public AbsolutePath RootDirectory { get; internal set; }

    public IReadOnlyList<string> SelectedTargetNames { get; internal set; } = null!;
    public IBuildOutput BuildOutput { get; internal set; } = null!;

    public void Add<T>(object key, T value) where T : class
    {
        _context.Add(key, value);
    }

    public T GetOrAdd<T>(object key, Func<Context, T> factory) where T : class
    {
        ref T value = ref Unsafe.As<object, T>(ref CollectionsMarshal.GetValueRefOrAddDefault(_context, key, out bool exists)!);
        if (!exists)
        {
            value = factory(this);
        }

        return value;
    }

    public void Set<T>(object key, T value) where T : class
    {
        _context[key] = value;
    }

    public bool TryGet<T>(object key, [NotNullWhen(true)] out T? value)
    {
        if (_context.TryGetValue(key, out var valueObject) && valueObject is T castedValue)
        {
            value = castedValue;
            return true;
        }

        value = default;
        return false;
    }

    public CommandLineTool SearchForCommand(string command, IBuildOutput? defaultBuildOutput = null)
    {
        string? path;

        if (OperatingSystem.IsWindows())
        {
            string? pathExt = Environment.GetEnvironmentVariable("PATHEXT");
            if (pathExt != null)
            {
                var split = (pathExt + ";.DLL").Split(';', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < split.Length; ++i)
                {
                    split[i] = $"{command}{split[i]}";
                }

                path = FileSearch.SearchPath(split);
            }
            else
            {
                path = FileSearch.SearchPath([$"{command}.exe", $"{command}.dll"]);
            }
        }
        else
        {
            path = FileSearch.SearchPath([command, $"{command}.dll"]);
        }

        return path != null ?
            CreateCommand(path, defaultBuildOutput) :
            throw new FileNotFoundException($"Command '{command}' was not found in path.");
    }

    public CommandLineTool SearchForTool(string tool, IBuildOutput? defaultBuildOutput = null)
    {
        foreach (var path in GetToolsJsonPaths())
        {
            var config = JsonSerializer.Deserialize<DotNetToolsJson>(File.ReadAllText(path), JsonSerializerOptions.Web);

            if (config != null &&
                config.Tools?.TryGetValue(tool.ToLowerInvariant(), out var toolConfig) == true &&
                toolConfig!.Commands?.TryGetFirstOrDefault(out string? cmd) == true &&
                !string.IsNullOrWhiteSpace(cmd))
            {
                var dotNetTool = new DotNet(this).Tool;
                return new CommandLineTool(dotNetTool.FileName, defaultBuildOutput ?? dotNetTool.DefaultBuildOutput, $"tool run {cmd} --");
            }

            if (config?.IsRoot == true)
            {
                break;
            }
        }

        throw new FileNotFoundException($"Could not find configuration for tool {tool}.");
    }

    public CommandLineTool CommandFromFullPath(string path, IBuildOutput? defaultBuildOutput = null)
    {
        if (!Path.IsPathFullyQualified(path))
        {
            throw new ArgumentException("Not a full path", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File '{path}' does not exists.");
        }

        return CreateCommand(path, defaultBuildOutput);
    }

    private CommandLineTool CreateCommand(string path, IBuildOutput? defaultBuildOutput)
    {
        if (!path.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
        {
            return new CommandLineTool(path, defaultBuildOutput ?? BuildOutput);
        }

        // "fullPathTo/dotnet.exe theApp.dll --" so that anything after are theApp.dll arguments and not dotnet.exe
        var dotNetTool = new DotNet(this).Tool;
        return new CommandLineTool(dotNetTool.FileName, defaultBuildOutput ?? dotNetTool.DefaultBuildOutput, $"{path} --");

    }

    private IEnumerable<AbsolutePath> GetToolsJsonPaths()
    {
        RelativePath configToolsJsonPath = OperatingSystem.IsWindows() ? ".config\\dotnet-tools.json" : ".config/dotnet-tools.json";
        RelativePath toolsJsonPath = "dotnet-tools.json";
        AbsolutePath? path = RootDirectory;

        while (path.HasValue)
        {
            var fullPath = path.Value / configToolsJsonPath;
            if (fullPath.FileExists())
            {
                yield return fullPath;
            }

            fullPath = path.Value / toolsJsonPath;
            if (fullPath.FileExists())
            {
                yield return fullPath;
            }

            path = path.Value.Parent;
        }
    }
}