namespace Bacon.Build;

public static class BuilderExtensions
{
    public static IEnumerable<TArguments> CombineWith<TBuilder, TArguments, TValue>(
        this TBuilder builder,
        IEnumerable<TValue> values,
        Func<TBuilder, TValue, TArguments> configure)
        where TBuilder : IArgumentsBuilder<TBuilder, TArguments>
        where TArguments : Arguments
    {
        foreach (TValue value in values)
        {
            yield return configure(builder.Clone(), value);
        }
    }
}