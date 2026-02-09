using System.Text;

namespace Bacon.Build;

public sealed class EnvironmentVariableConfigurationSource<T> : IConfigurationSource<T> where T : class
{
    public Task ApplyAsync(T context, IReadOnlyList<InputInfo> inputsInfo, BuildConfiguration buildConfiguration)
    {
        foreach (InputInfo inputInfo in inputsInfo)
        {
            foreach (string name in GetNames(inputInfo.Name))
            {
                string? value = Environment.GetEnvironmentVariable(name);
                if (value != null)
                {
                    inputInfo.ApplyValue(context, value);
                    break;
                }
            }
        }

        return Task.CompletedTask;
    }

    private static IEnumerable<string> GetNames(string name)
    {
        var snake = ToSnakeCase(name);

        if (OperatingSystem.IsWindows())
        {
            return snake != name ? [snake, name] :  [snake];
        }

        return new HashSet<string>
        {
            snake.ToUpperInvariant(),
            snake,
            snake.ToLowerInvariant(),
            name,
            name.ToUpperInvariant(),
            name.ToLowerInvariant()
        };
    }

    private static string ToSnakeCase(string name)
    {
        // Could be improved ... I guess if you already named your property with _, it is already in snake case ...
        if (name.Contains('_'))
        {
            return name;
        }

        var sb = new StringBuilder(name.Length + 8); // Just a guess
        bool previousWasUpper = true;
        foreach (char c in name)
        {
            if (!char.IsUpper(c))
            {
                previousWasUpper = false;
            }
            else if (!previousWasUpper)
            {
                sb.Append('_');
            }

            sb.Append(c);
        }

        return sb.ToString();
    }
}