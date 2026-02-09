namespace Bacon.Build;

public interface IApply<in T>
{
    void Apply(T context);
}