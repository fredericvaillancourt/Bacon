using Bacon.Build;

namespace Bacon.Tools;

[Tool("dotnet-reportgenerator-globaltool", ToolLocation.Tool)]
[Syntax("{args}")]
public partial class ReportGeneratorArguments : Arguments
{
    public string[] Arguments { get; }

    // Temporary hack ... famous last words
    public class ArgumentsBuilder
    {
        public string? Reports { get; set; }

        public ArgumentsBuilder SetReports(AbsolutePath? value)
        {
            Reports = value;
            return this;
        }

        public ArgumentsBuilder SetReports(IEnumerable<AbsolutePath> values)
        {
            Reports = string.Join(';', values);
            if (string.IsNullOrWhiteSpace(Reports))
            {
                Reports = null;
            }

            return this;
        }

        public ArgumentsBuilder ResetReports()
        {
            Reports = null;
            return this;
        }

        public string? TargetDirectory { get; set; }

        public ArgumentsBuilder SetTargetDirectory(AbsolutePath? value)
        {
            TargetDirectory = value;
            return this;
        }

        public ArgumentsBuilder ResetTargetDirectory()
        {
            TargetDirectory = null;
            return this;
        }

        public string? ReportTypes { get; set; }

        public ArgumentsBuilder SetReportTypes(string? value)
        {
            ReportTypes = value;
            return this;
        }

        public ArgumentsBuilder SetReportTypes(IEnumerable<string> values)
        {
            ReportTypes = string.Join(';', values);
            if (string.IsNullOrWhiteSpace(ReportTypes))
            {
                ReportTypes = null;
            }

            return this;
        }

        public ArgumentsBuilder ResetReportTypes()
        {
            ReportTypes = null;
            return this;
        }

        public string[] Build()
        {
            var results = new List<string>();

            if (Reports != null)
            {
                results.Add($"-reports:{Reports}");
            }

            if (TargetDirectory != null)
            {
                results.Add($"-targetdir:{TargetDirectory}");
            }

            if (ReportTypes != null)
            {
                results.Add($"-reporttypes:{ReportTypes}");
            }

            return results.ToArray();
        }

        public static implicit operator string[](ArgumentsBuilder argumentsBuilder)
        {
            return argumentsBuilder.Build();
        }
    }
}