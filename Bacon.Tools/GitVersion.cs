using System.Text;
using System.Text.Json;
using Bacon.Build;

namespace Bacon.Tools;

public partial class GitVersion
{
    public GitVersionResult GetVersions()
    {
        var results = this.Execute(static c => c);

        var sb = new StringBuilder();
        foreach (var output in results.Output)
        {
            if (output.Kind == OutputKind.Out)
            {
                sb.AppendLine(output.Line);
            }
        }

        return JsonSerializer.Deserialize<GitVersionResult>(sb.ToString()) ?? throw new InvalidOperationException("Cannot deserialize");
    }
}