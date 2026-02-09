namespace Bacon.Build;

[System.Diagnostics.DebuggerDisplay("{Description,nq}")]
public abstract class RequiresInfo<T>(string description)
{
    public string Description { get; } = description;

    public abstract ValueTask<bool> EvaluateAsync(T context);
}

internal sealed class RequiresClassContextInfo<T, TReturn>(string description, Func<T, TReturn?> func)
    : RequiresInfo<T>(description)
    where TReturn : class
{
    public override ValueTask<bool> EvaluateAsync(T context)
    {
        return new ValueTask<bool>(func(context) != null);
    }
}

internal sealed class RequiresStructContextInfo<T, TReturn>(string description, Func<T, TReturn?> func)
    : RequiresInfo<T>(description)
    where TReturn : struct
{
    public override ValueTask<bool> EvaluateAsync(T context)
    {
        return new ValueTask<bool>(func(context).HasValue);
    }
}

internal sealed class RequiresStringContextInfo<T>(string description, Func<T, string?> func) : RequiresInfo<T>(description)
{
    public override ValueTask<bool> EvaluateAsync(T context)
    {
        return new ValueTask<bool>(!string.IsNullOrEmpty(func(context)));
    }
}

internal sealed class RequiresBoolContextInfo<T>(string description, Func<T, bool> func) : RequiresInfo<T>(description)
{
    public override ValueTask<bool> EvaluateAsync(T context)
    {
        return new ValueTask<bool>(func(context));
    }
}

internal sealed class RequiresClassInfo<T, TReturn>(string description, Func<TReturn?> func)
    : RequiresInfo<T>(description)
    where TReturn : class
{
    public override ValueTask<bool> EvaluateAsync(T _)
    {
        return new ValueTask<bool>(func() != null);
    }
}

internal sealed class RequiresStructInfo<T, TReturn>(string description, Func<TReturn?> func)
    : RequiresInfo<T>(description)
    where TReturn : struct
{
    public override ValueTask<bool> EvaluateAsync(T _)
    {
        return new ValueTask<bool>(func().HasValue);
    }
}

internal sealed class RequiresStringInfo<T>(string description, Func<string?> func) : RequiresInfo<T>(description)
{
    public override ValueTask<bool> EvaluateAsync(T _)
    {
        return new ValueTask<bool>(!string.IsNullOrEmpty(func()));
    }
}

internal sealed class RequiresBoolInfo<T>(string description, Func<bool> func) : RequiresInfo<T>(description)
{
    public override ValueTask<bool> EvaluateAsync(T _)
    {
        return new ValueTask<bool>(func());
    }
}

internal sealed class RequiresAsyncClassContextInfo<T, TReturn>(string description, Func<T, Task<TReturn?>> func)
    : RequiresInfo<T>(description)
    where TReturn : class
{
    public override async ValueTask<bool> EvaluateAsync(T context)
    {
        return (await func(context)) != null;
    }
}

internal sealed class RequiresAsyncStructContextInfo<T, TReturn>(string description, Func<T, Task<TReturn?>> func)
    : RequiresInfo<T>(description)
    where TReturn : struct
{
    public override async ValueTask<bool> EvaluateAsync(T context)
    {
        return (await func(context)).HasValue;
    }
}

internal sealed class RequiresAsyncStringContextInfo<T>(string description, Func<T, Task<string?>> func) : RequiresInfo<T>(description)
{
    public override async ValueTask<bool> EvaluateAsync(T context)
    {
        return !string.IsNullOrEmpty(await func(context));
    }
}

internal sealed class RequiresAsyncBoolContextInfo<T>(string description, Func<T, Task<bool>> func) : RequiresInfo<T>(description)
{
    public override ValueTask<bool> EvaluateAsync(T context)
    {
        return new ValueTask<bool>(func(context));
    }
}

internal sealed class RequiresAsyncClassInfo<T, TReturn>(string description, Func<Task<TReturn?>> func)
    : RequiresInfo<T>(description)
    where TReturn : class
{
    public override async ValueTask<bool> EvaluateAsync(T _)
    {
        return await func() != null;
    }
}

internal sealed class RequiresAsyncStructInfo<T, TReturn>(string description, Func<Task<TReturn?>> func)
    : RequiresInfo<T>(description)
    where TReturn : struct
{
    public override async ValueTask<bool> EvaluateAsync(T _)
    {
        return (await func()).HasValue;
    }
}

internal sealed class RequiresAsyncStringInfo<T>(string description, Func<Task<string?>> func) : RequiresInfo<T>(description)
{
    public override async ValueTask<bool> EvaluateAsync(T _)
    {
        return !string.IsNullOrEmpty(await func());
    }
}

internal sealed class RequiresAsyncBoolInfo<T>(string description, Func<Task<bool>> func) : RequiresInfo<T>(description)
{
    public override ValueTask<bool> EvaluateAsync(T _)
    {
        return new ValueTask<bool>(func());
    }
}