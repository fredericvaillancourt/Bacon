using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} update {args}")]
public abstract partial class DotNetNugetUpdateArguments : DotNetNugetArguments
{
}