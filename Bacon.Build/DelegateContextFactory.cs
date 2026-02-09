namespace Bacon.Build;

public sealed class DelegateContextFactory<T>(Func<T> factory) : IFactory<T> where T : new()
{
    public T Create() => factory();
}