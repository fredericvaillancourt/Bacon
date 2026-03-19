using Microsoft.CodeAnalysis;

namespace Bacon.Generator;

internal static class IncrementalValuesProviderExtensions
{
    public static IncrementalValuesProvider<T> NotNull<T>(this IncrementalValuesProvider<T?> provider)
    {
        return provider.Where(static s => s != null).Select<T?, T>(static (s, _) => s!);
    }
}