namespace Bacon.Build;

public interface IArgumentsBuilder<out TBuilder, out TArguments>
{
    TBuilder Clone();
    TArguments Build();
}