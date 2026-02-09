using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace Bacon.Build;

public sealed class Build<T> where T : Context
{
    private readonly IFactory<T> _contextFactory;
    private readonly IReadOnlyCollection<IConfigurationSource<T>> _configurationSources;
    private readonly IReadOnlyList<Func<T, Task>>? _configureContext;
    private readonly IBuildOutput _buildOutput;

    public IReadOnlyList<Target<T>> Targets { get; }
    public Target<T>? DefaultTarget { get; }

    private Build(
        IReadOnlyList<Target<T>> targets,
        Target<T>? defaultTarget,
        IFactory<T> contextFactory,
        IReadOnlyCollection<IConfigurationSource<T>> configurationSources,
        IReadOnlyList<Func<T, Task>>? configureContext = null,
        IBuildOutput? buildOutput = null)
    {
        _contextFactory = contextFactory;
        _configurationSources = configurationSources;
        _configureContext = configureContext;
        _buildOutput = buildOutput ?? (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ENV")) ? ConsoleBuildOutput.Instance : GitHubBuildOutput.Instance);
        Targets = targets;
        DefaultTarget = defaultTarget;
    }

    public async Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        if (args is ["--help"])
        {
            return Help();
        }

        var context = _contextFactory.Create();
        context.BuildOutput = _buildOutput;
        context.RootDirectory = Directory.GetCurrentDirectory();

        var buildConfiguration = new BuildConfiguration();
        var inputs = GetInputs();
        foreach (var configurationSource in _configurationSources.Reverse())
        {
            await configurationSource.ApplyAsync(context, inputs, buildConfiguration);
        }

        if (buildConfiguration.Targets.Count == 0 && DefaultTarget == null)
        {
            _buildOutput.WriteError("No target to build");
            return 1;
        }

        if (!TryPrepareTargets(buildConfiguration.Targets, out var sorted))
        {
            return 2;
        }

        context.SelectedTargetNames = sorted.Select(static t => t.Name).ToArray();

        if (_configureContext != null)
        {
            foreach (var action in _configureContext)
            {
                await action(context);
            }
        }

        if (!await ValidateRequiresAsync(context, sorted))
        {
            return 3;
        }

        return await BuildTargetAsync(context, sorted, cancellationToken);
    }

    private int Help()
    {
        var parameters = GetParameters().Select(static p => new{Name = p.Attribute.Name ?? p.Property.Name, Description = p.Attribute.Description}).ToArray();
        if (parameters.Length > 0)
        {
            _buildOutput.WriteInformation("Parameters:");
            int maxNameLength = parameters.Max(static n => n.Name.Length);

            foreach (var parameter in parameters)
            {
                string extra = parameter.Description != null ?
                    $":{new string(' ', maxNameLength - parameter.Name.Length)} {parameter.Description}" :
                    "";
                _buildOutput.WriteInformation($"    {parameter.Name}{extra}");
            }

            _buildOutput.WriteInformation("");
        }

        _buildOutput.WriteInformation("Targets:");
        var graph = BuildGraph(Targets);
        var sorted = SortGraph(graph);
        foreach (Node<Target<T>> node in sorted)
        {
            if (node.Data.Unlisted)
            {
                continue;
            }

            _buildOutput.WriteInformation($"    {node.Data.Name}{(node.Data == DefaultTarget ? " (Default)" : "")}");
        }

        return 0;
    }

    private async Task<bool> ValidateRequiresAsync(T context, Target<T>[] sorted)
    {
        bool result = true;

        foreach (var target in sorted)
        {
            foreach (var require in target.Requires)
            {
                if (await require.EvaluateAsync(context))
                {
                    continue;
                }

                _buildOutput.WriteError($"Missing required: {require.Description}");
                result = false;
            }
        }

        return result;
    }

    private async Task<int> BuildTargetAsync(T context, Target<T>[] sorted, CancellationToken cancellationToken)
    {
        var results = new TargetResult[sorted.Length];

        for (int i = 0; i < sorted.Length; ++i)
        {
            results[i] = new TargetResult(sorted[i].Name, TargetStatus.Waiting, null);
        }

        for (int i = 0; i < sorted.Length; ++i)
        {
            var t = sorted[i];
            
            if (t.OnlyWhen.Any(w => !w(context)))
            {
                results[i] = new TargetResult(sorted[i].Name, TargetStatus.Skipped, null);
                continue;
            }

            _buildOutput.BeginTarget(t.Name);

            var sw = Stopwatch.StartNew();
            try
            {
                foreach (Func<T, CancellationToken, Task> action in t.Executes)
                {
                    await action(context, cancellationToken);
                }
            }
            catch
            {
                _buildOutput.EndTarget(t.Name);
                results[i] = new TargetResult(sorted[i].Name, TargetStatus.Failed, sw.Elapsed);
                _buildOutput.BuildCompleted(results);
                throw;
            }

            results[i] = new TargetResult(t.Name, TargetStatus.Succeeded, sw.Elapsed);
            _buildOutput.EndTarget(t.Name);
        }

        _buildOutput.BuildCompleted(results);
        return 0;
    }

    private static string[] FillContext(T context, ReadOnlySpan<string> args)
    {
        var parameters = GetParameters().ToDictionary(static k => k.Attribute.Name ?? k.Property.Name, static v => v.Property);

        var targets = new List<string>();
        var p = new CommandLineParser(args);
        bool consumeAsToggle = false;
        PropertyInfo? property = null;
        while (p.MoveNext(consumeAsToggle))
        {
            if (p.Value != null)
            {
                if (property == null)
                {
                    targets.Add(p.Value);
                }
                else
                {
                    property.SetValue(context, Convert.ChangeType(p.Value, property.PropertyType));
                }

                property = null;
                consumeAsToggle = false;
            }
            else
            {
                if (!parameters.TryGetValue(p.Name!, out property))
                {
                    throw new InvalidOperationException($"Command line argument {p.Name} not found on context.");
                }

                consumeAsToggle = property.PropertyType == typeof(bool);
                if (consumeAsToggle)
                {
                    property.SetValue(context, true);
                }
            }
        }

        context.RootDirectory = Directory.GetCurrentDirectory();

        return targets.ToArray();
    }

    private static IEnumerable<(PropertyInfo Property, InputAttribute Attribute)> GetParameters()
    {
        foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
        {
            var attribute = propertyInfo.GetCustomAttribute<InputAttribute>();
            if (attribute == null)
            {
                continue;
            }

            yield return (propertyInfo, attribute);
        }
    }

    private bool TryPrepareTargets(IReadOnlyList<string> targetNames, [NotNullWhen(true)] out Target<T>[]? sorted)
    {
        var dict = Targets.ToDictionary(static k => k.Name);

        var selected = new List<Target<T>>();

        if (targetNames.Count != 0)
        {
            foreach (string targetName in targetNames)
            {
                //TODO: Could improve by listing ALL unknown and not just the first one
                if (!dict.TryGetValue(targetName, out var t))
                {
                    _buildOutput.WriteError($"Could not find target {targetName}");
                    sorted = null;
                    return false;
                }

                selected.Add(t);
            }
        }
        else
        {
            selected.Add(DefaultTarget!);
        }

        var toRun = TargetsToRun(selected);
        var graph = BuildGraph(toRun);
        sorted = SortGraph(graph).Select(static n => n.Data).Reverse().ToArray();
        return true;
    }

    private static List<Node<Target<T>>> BuildGraph(IEnumerable<Target<T>> targets)
    {
        var nodes = new List<Node<Target<T>>>();
        foreach (var target in targets)
        {
            nodes.Add(new Node<Target<T>>(target));
        }

        var dictionary = nodes.ToDictionary(static k => k.Data);

        foreach (var node in nodes)
        {
            foreach (var dependsOn in node.Data.DependsOn)
            {
                node.AddEdgeTo(dictionary[dependsOn]);
            }

            foreach (var after in node.Data.After)
            {
                if (dictionary.TryGetValue(after, out var a))
                {
                    node.AddEdgeTo(a);
                }
            }

            foreach (var before in node.Data.Before)
            {
                if (dictionary.TryGetValue(before, out var b))
                {
                    b.AddEdgeTo(node);
                }
            }
        }

        return nodes;
    }

    private static List<Node<Target<T>>> SortGraph(List<Node<Target<T>>> graph)
    {
        var l = new List<Node<Target<T>>>(graph.Count);
        var s = new Stack<Node<Target<T>>>(graph.Count);

        foreach (var node in graph)
        {
            if (node.HasIncomingEdge)
            {
                continue;
            }

            s.Push(node);
        }

        int removedCount = s.Count;

        while (s.TryPop(out var n))
        {
            l.Add(n);
            removedCount += n.Remove(s);
        }

        if (removedCount != graph.Count)
        {
            throw new InvalidOperationException("Circular dependency");
        }

        return l;
    }

    private HashSet<Target<T>> TargetsToRun(IEnumerable<Target<T>> selected)
    {
        var result = new HashSet<Target<T>>();
        foreach (var s in selected)
        {
            TargetsToRun(s, result);
        }
        
        return result;
    }

    private static void TargetsToRun(Target<T> selected, HashSet<Target<T>> result)
    {
        if (!result.Add(selected))
        {
            return;
        }

        foreach (Target<T> targetInfo in selected.DependsOn)
        {
            TargetsToRun(targetInfo, result);
        }
    }

    private static IReadOnlyList<InputInfo> GetInputs()
    {
        var inputs = new List<InputInfo>();

        foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
        {
            var attribute = propertyInfo.GetCustomAttribute<InputAttribute>();
            if (attribute == null)
            {
                continue;
            }

            inputs.Add(new(propertyInfo, attribute, attribute.Name ?? propertyInfo.Name));
        }

        return inputs;
    }

    public class Builder
    {
        private readonly List<Func<T, Task>> _context = new();

        public List<Target<T>> Targets { get; } = new();
        public Target<T>? DefaultTarget { get; set; }

        public List<IConfigurationSource<T>> ConfigurationSources { get; } = new()
        {
            new CommandLineConfigurationSource<T>(Environment.GetCommandLineArgs()[1..])
        };

        public IFactory<T>? ContextFactory { get; set; }

        public Builder AddTarget(Target<T> target)
        {
            Targets.Add(target);
            return this;
        }

        public Builder AddTarget(string name, Func<Target<T>.Builder, Target<T>> configure, out Target<T> target)
        {
            return AddTarget(target = configure(new Target<T>.Builder(name)));
        }

        public Builder AddTarget(string name, Func<Target<T>.Builder, Target<T>> configure)
        {
            return AddTarget(name, configure, out _);
        }

        public Builder SetDefaultTarget(Target<T> target)
        {
            DefaultTarget = target;
            return this;
        }

        public Builder Context(Action<T> configure)
        {
            _context.Add(c =>
            {
                configure(c);
                return Task.CompletedTask;
            });

            return this;
        }

        public Builder Context(Func<T, Task> configure)
        {
            _context.Add(configure);
            return this;
        }

        public Builder ClearConfigurationSource()
        {
            ConfigurationSources.Clear();
            return this;
        }

        public Builder AddConfigurationSource(params ReadOnlySpan<IConfigurationSource<T>> sources)
        {
            ConfigurationSources.AddRange(sources);
            return this;
        }

        public Builder AddCommandLineConfigurationSource(string[] args)
        {
            ConfigurationSources.Add(new CommandLineConfigurationSource<T>(args));
            return this;
        }

        public Builder AddEnvironmentVariableConfigurationSource()
        {
            ConfigurationSources.Add(new EnvironmentVariableConfigurationSource<T>());
            return this;
        }

        public Builder AddJsonConfigurationSource(string filename, JsonSerializerOptions? options = null)
        {
            ConfigurationSources.Add(new JsonConfigurationSource<T>(filename, options));
            return this;
        }

        public Builder SetContextFactory(IFactory<T> contextFactory)
        {
            ContextFactory = contextFactory;
            return this;
        }

        public Build<T> Build()
        {
            return new Build<T>(
                Targets.ToArray(),
                DefaultTarget,
                ContextFactory ?? GetContextFactory(),
                ConfigurationSources.ToArray(),
                _context.ToArray());
        }

        public static implicit operator Build<T>(Builder builder)
        {
            return builder.Build();
        }

        private static IFactory<T> GetContextFactory()
        {
            var type = typeof(T);
            if (type is { IsAbstract: false, IsClass: true })
            {
                var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
                if (constructor != null)
                {
                    var factoryType = typeof(NewContextFactory<>).MakeGenericType(typeof(T));
                    return (IFactory<T>)Activator.CreateInstance(factoryType)!;
                }
            }

            throw new InvalidOperationException("Missing context factory");
        }
    }
}