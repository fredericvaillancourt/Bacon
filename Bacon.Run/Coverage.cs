using System.Xml.Serialization;

namespace Bacon.Run;

[XmlRoot("coverage", Namespace = "")]
public class Coverage
{
    [XmlAttribute("version")]
    public required string Version { get; set; }

    [XmlElement("file")]
    public List<CoverageFile> Files { get; } = new();
}

public class CoverageFile
{
    [XmlAttribute("path")]
    public required string Path { get; set; }

    [XmlElement("lineToCover")]
    public List<CoverageLineToCover> LineToCovers { get; } = new();
}

public class CoverageLineToCover
{
    [XmlAttribute("lineNumber")]
    public int LineNumber { get; set; }

    [XmlAttribute("covered")]
    public bool Covered { get; set; }

    [XmlIgnore]
    public int? BranchesToCover { get; set; }

    [XmlIgnore]
    public int? CoveredBranches { get; set; }

    [XmlAttribute(AttributeName = "branchesToCover")]
    public string? BranchesToCoverSerializable
    {
        get => BranchesToCover?.ToString()!;
        set => BranchesToCover = value != null ? int.Parse(value) : null;
    }

    [XmlAttribute(AttributeName = "coveredBranches")]
    public string? CoveredBranchesSerializable
    {
        get => CoveredBranches?.ToString()!;
        set => CoveredBranches = value != null ? int.Parse(value) : null;
    }
}

internal record CoverageResult(string Filename, int[] Lines);