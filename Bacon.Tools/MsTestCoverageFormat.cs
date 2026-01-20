using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum MsTestCoverageFormat
{
    [EnumName("coverage")] Coverage,
    [EnumName("xml")] Xml,
    [EnumName("cobertura")] Cobertura
}