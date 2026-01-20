namespace Bacon.Tools;

public record GitVersionResult(
    string AssemblySemFileVer,
    string AssemblySemVer,
    string BranchName,
    int? BuildMetaData,
    string CommitDate,
    int CommitsSinceVersionSource,
    string EscapedBranchName,
    string FullBuildMetaData,
    string FullSemVer,
    string InformationalVersion,
    int Major,
    string MajorMinorPatch,
    int Minor,
    int Patch,
    string PreReleaseLabel,
    string PreReleaseLabelWithDash,
    int PreReleaseNumber,
    string PreReleaseTag,
    string PreReleaseTagWithDash,
    string SemVer,
    string Sha,
    string ShortSha,
    int? UncommittedChanges,
    string VersionSourceSha,
    int WeightedPreReleaseNumber
);