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

    public ITool<string, Result> SearchForCommand(string command, IBuildOutput? defaultBuildOutput = null)
    {
        string? path = FileSearch.SearchPath(OperatingSystem.IsWindows() ?
                [$"{command}.exe", $"{command}.dll"] :
                [command, $"{command}.dll"]);

        if (path == null)
        {
            throw new FileNotFoundException($"Command '{command}' was not found in path.");
        }

        return CreateCommand(path, defaultBuildOutput);
    }

    public ITool<string, Result> SearchForTool(string tool, IBuildOutput? defaultBuildOutput = null)
    {
        //TODO: The real algo I think is to merge all those while going up ... and maybe roots too ...
        var path = GetToolsJsonPath();

        if (!path.HasValue)
        {
            throw new FileNotFoundException("Could not find tools configuration.", path);
        }

        var config = JsonSerializer.Deserialize<DotNetToolsJson>(File.ReadAllText(path), JsonSerializerOptions.Web);

        if (config == null)
        {
            throw new InvalidDataException("Could not deserialize tools configuration.");
        }

        if (!config.Tools.TryGetValue(tool.ToLowerInvariant(), out var toolConfig))
        {
            throw new InvalidOperationException($"Could not find tool {tool}.");
        }

        string? cmd = toolConfig.Commands.FirstOrDefault();
        if (cmd == null)
        {
            throw new InvalidDataException("Could not find tool command");
        }

        return new ComposedCommandLineTool(new DotNet(this).Tool, $"tool run {cmd} --", defaultBuildOutput);
    }

    public ITool<string, Result> CommandFromFullPath(string path, IBuildOutput? defaultBuildOutput = null)
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

    private ITool<string, Result> CreateCommand(string path, IBuildOutput? defaultBuildOutput)
    {
        return path.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) ?
            // "fullPathTo/dotnet.exe theApp.dll --" so that anything after are theApp.dll arguments and not dotnet.exe
            new ComposedCommandLineTool(new DotNet(this).Tool, $"{path} --", defaultBuildOutput) :
            new CommandLineTool(path, defaultBuildOutput ?? BuildOutput);
    }

    private AbsolutePath? GetToolsJsonPath()
    {
        RelativePath toolsJsonPath = OperatingSystem.IsWindows() ? ".config\\dotnet-tools.json" : ".config/dotnet-tools.json";
        AbsolutePath? path = RootDirectory;

        while (path.HasValue)
        {
            var fullPath = path.Value / toolsJsonPath;
            if (fullPath.FileExists())
            {
                return fullPath;
            }

            path = path.Value.Parent;
        }

        return null;
    }
}