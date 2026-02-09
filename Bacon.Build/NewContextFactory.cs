namespace Bacon.Build;

public sealed class NewContextFactory<T> : IFactory<T> where T : new()
{
    public T Create() => new();
}