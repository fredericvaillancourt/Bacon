using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} pack {args}")]
public partial class OctopusPackArguments : OctopusArguments
{
    [Parameter("id")]
    public string Id { get; }

    [Parameter("format")]
    public OctopusPackFormat? PackFormat { get; }

    [Parameter("version")]
    public string? Version { get; }

    [Parameter("outFolder")]
    public string? OutputFolder { get; }

    [Parameter("basePath")]
    public string? BasePath { get; }

    [Parameter("verbose")]
    public bool Verbose { get; }

    [Parameter("author")]
    public string? Author { get; }

    [Parameter("title")]
    public string? Title { get; }

    [Parameter("description")]
    public string? Description { get; }

    [Parameter("releaseNotes")]
    public string? ReleaseNotes { get; }

    [Parameter("releaseNotesFile")]
    public string? ReleaseNotesFile { get; }

    [Parameter("compressionLevel")]
    public OctopusPackCompressionLevel? CompressionLevel { get; }

    [Parameter("include")]
    public string? Include { get; } //TODO: This is a collection

    [Parameter("overwrite")]
    public bool Overwrite { get; }
}