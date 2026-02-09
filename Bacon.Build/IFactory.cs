namespace Bacon.Build;

public interface IFactory<out T>
{
    T Create();
}