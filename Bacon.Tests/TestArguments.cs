using Bacon.Build;

namespace Bacon.Tests;

[Syntax("")]
[Tool("dotnet")] // A tool that we are pretty sure is available ...
public abstract partial class TestArguments : Arguments
{
}