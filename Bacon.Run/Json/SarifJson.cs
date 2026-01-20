namespace Bacon.Run.Json;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// Auto generated with [Paste Special : JSON as Classes]

// Nullable was "fixed" trying to follow the standard. Some classes are duplicated and should be reused rather than redefined but that cleanup was NOT done
// https://docs.oasis-open.org/sarif/sarif/v2.1.0/errata01/os/sarif-v2.1.0-errata01-os-complete.html

internal class Sarif
{
    public required string Version { get; set; }
    public Runs[]? Runs { get; set; }
}

internal class Runs
{
    public Results[]? Results { get; set; }
    public required Tool Tool { get; set; }
    public Invocations[]? Invocations { get; set; }
    public VersionControlProvenance[]? VersionControlProvenance { get; set; }
    public OriginalUriBaseIds? OriginalUriBaseIds { get; set; }
    public Artifacts[]? Artifacts { get; set; }
    public string? ColumnKind { get; set; }
}

internal class Results
{
    public string? RuleId { get; set; }
    public int? RuleIndex { get; set; }
    public required Message Message { get; set; }
    public Locations[]? Locations { get; set; }
    public PartialFingerprints? PartialFingerprints { get; set; }
}

internal class Message
{
    public string? Text { get; set; }
}

internal class Locations
{
    public PhysicalLocation? PhysicalLocation { get; set; }
}

internal class PhysicalLocation
{
    public ArtifactLocation? ArtifactLocation { get; set; }
    public Region? Region { get; set; }
}

internal class ArtifactLocation
{
    public string? Uri { get; set; }
    public string? UriBaseId { get; set; }
    public int? Index { get; set; }
}

internal class Region
{
    public int? StartLine { get; set; }
    public int? StartColumn { get; set; }
    public int? EndLine { get; set; }
    public int? EndColumn { get; set; }
    public int? CharOffset { get; set; }
    public int? CharLength { get; set; }
}

internal class PartialFingerprints
{
    public string? ContextRegionHashV1 { get; set; }
}

internal class Tool
{
    public required Driver Driver { get; set; }
}

internal class Driver
{
    public required string Name { get; set; }
    public string? Organization { get; set; }
    public string? FullName { get; set; }
    public string? SemanticVersion { get; set; }
    public string? InformationUri { get; set; }
    public Rules[]? Rules { get; set; }
}

internal class Rules
{
    public required string Id { get; set; }
    public Help? Help { get; set; }
    public ShortDescription? ShortDescription { get; set; }
    public string? HelpUri { get; set; }
    public Relationships[]? Relationships { get; set; }
}

internal class Help
{
    public required string Text { get; set; }
}

internal class ShortDescription
{
    public required string Text { get; set; }
}

internal class Relationships
{
    public required Target Target { get; set; }
    public string[]? Kinds { get; set; }
}

internal class Target
{
    public required string Id { get; set; }
}

internal class Invocations
{
    public bool ExecutionSuccessful { get; set; }
}

internal class VersionControlProvenance
{
    public required string RepositoryUri { get; set; }
    public string? RevisionId { get; set; }
    public string? Branch { get; set; }
    public MappedTo? MappedTo { get; set; }
}

internal class MappedTo
{
    public string? UriBaseId { get; set; }
}

internal class OriginalUriBaseIds
{
    public SolutionDir? SolutionDir { get; set; }
}

internal class SolutionDir
{
    public string? Uri { get; set; }
    public Description? Description { get; set; }
}

internal class Description
{
    public string? Text { get; set; }
}

internal class Artifacts
{
    public ArtifactLocation? Location { get; set; }
    public Hashes? Hashes { get; set; }
}

internal class Hashes
{
    public string? Md5 { get; set; }
    public string? Sha1 { get; set; }
    public string? Sha256 { get; set; }
}