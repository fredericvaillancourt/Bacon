namespace Bacon.Build;

public interface ITool
{
    // Name? TInput, TOutput?
}

public interface ITool<in TInput, out TOutput> : ITool
{
    TOutput Execute(TInput input, IBuildOutput? buildOutput = null);
}