namespace Bacon.Build;

public sealed class CommandLineConfigurationSource<T>(string[] args) : IConfigurationSource<T> where T : class
{
    public Task ApplyAsync(T context, IReadOnlyList<InputInfo> inputsInfo, BuildConfiguration buildConfiguration)
    {
        var parameters = inputsInfo.ToDictionary(static k => k.Name);

        var p = new CommandLineParser(args);
        bool consumeAsToggle = false;
        InputInfo? current = null;
        while (p.MoveNext(consumeAsToggle))
        {
            if (p.Value != null)
            {
                if (current == null)
                {
                    buildConfiguration.Targets.Add(p.Value);
                }
                else
                {
                    current.ApplyValue(context, p.Value);
                }

                current = null;
                consumeAsToggle = false;
            }
            else
            {
                if (!parameters.TryGetValue(p.Name!, out current))
                {
                    throw new InvalidOperationException($"Command line argument {p.Name} not found on context.");
                }

                consumeAsToggle = current.Property.PropertyType == typeof(bool);
                if (consumeAsToggle)
                {
                    current.ApplyValue(context, true);
                }
            }
        }

        if (current != null)
        {
            //TODO: Should this be an exception?
            throw new InvalidOperationException($"Missing option value for {current.Name}");
        }

        return Task.CompletedTask;
    }
}