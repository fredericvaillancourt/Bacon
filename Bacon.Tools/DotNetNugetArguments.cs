using Bacon.Build;

namespace Bacon.Tools;

[Syntax("nuget {args}")]
public abstract partial class DotNetNugetArguments : DotNetArguments
{
}