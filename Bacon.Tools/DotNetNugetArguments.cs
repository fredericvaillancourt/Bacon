using Bacon.Build;

namespace Bacon.Tools;

[Syntax("{base} nuget {args}")]
public abstract partial class DotNetNugetArguments : DotNetArguments
{
}