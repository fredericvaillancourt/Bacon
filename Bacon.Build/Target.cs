using System.Runtime.CompilerServices;

namespace Bacon.Build;

[System.Diagnostics.DebuggerDisplay("{Name,nq}")]
public sealed record Target<T>(string Name, IReadOnlyList<Func<T, CancellationToken, Task>> Executes, IReadOnlyList<Target<T>> DependsOn, IReadOnlyList<Target<T>> After, IReadOnlyList<Target<T>> Before, IReadOnlyList<Predicate<T>> OnlyWhen, IReadOnlyList<RequiresInfo<T>> Requires, bool Unlisted) where T : Context
{
    public class Builder(string name)
    {
        public List<Func<T, CancellationToken, Task>> Executes { get; } = new();
        public List<Target<T>> DependsOn { get; } = new();
        public List<Target<T>> After { get; } = new();
        public List<Target<T>> Before { get; } = new();
        public List<Predicate<T>> OnlyWhen { get; } = new();
        public List<RequiresInfo<T>> Requires { get; } = new();

        public string Name { get; set; } = name;
        public bool Unlisted { get; set; }

        public Builder SetName(string name)
        {
            Name = name;
            return this;
        }

        public Builder AddExecutes(Action<T> action)
        {
            return AddExecutes((c, _) =>
            {
                action(c);
                return Task.CompletedTask;
            });
        }

        public Builder AddExecutes(Func<T, Task> action)
        {
            return AddExecutes((c, _) => action(c));
        }

        public Builder AddExecutes(Func<T, CancellationToken, Task> action)
        {
            Executes.Add(action);
            return this;
        }

        public Builder AddDependsOn(params ReadOnlySpan<Target<T>> targets)
        {
            DependsOn.AddRange(targets);
            return this;
        }

        public Builder AddAfter(params ReadOnlySpan<Target<T>> targets)
        {
            After.AddRange(targets);
            return this;
        }

        public Builder AddBefore(params ReadOnlySpan<Target<T>> targets)
        {
            Before.AddRange(targets);
            return this;
        }

        public Builder AddOnlyWhen(params ReadOnlySpan<Predicate<T>> predicate)
        {
            OnlyWhen.AddRange(predicate);
            return this;
        }

        public Builder EnableUnlisted()
        {
            Unlisted = true;
            return this;
        }

        public Builder DisableUnlisted()
        {
            Unlisted = false;
            return this;
        }

        public Builder AddRequires(RequiresInfo<T> require)
        {
            Requires.Add(require);
            return this;
        }

        public Builder AddRequires<TReturn>(Func<T, TReturn?> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : class
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresClassContextInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<T, TReturn?> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : struct
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresStructContextInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires(Func<T, string?> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresStringContextInfo<T>(description, expression));
        }

        public Builder AddRequires(Func<T, bool> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresBoolContextInfo<T>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<TReturn?> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : class
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresClassInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<TReturn?> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : struct
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresStructInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires(Func<string?> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresStringInfo<T>(description, expression));
        }

        public Builder AddRequires(Func<bool> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresBoolInfo<T>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<T, Task<TReturn?>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : class
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncClassContextInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<T, Task<TReturn?>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : struct
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncStructContextInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires(Func<T, Task<string?>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncStringContextInfo<T>(description, expression));
        }

        public Builder AddRequires(Func<T, Task<bool>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncBoolContextInfo<T>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<Task<TReturn?>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : class
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncClassInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires<TReturn>(Func<Task<TReturn?>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
            where TReturn : struct
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncStructInfo<T, TReturn>(description, expression));
        }

        public Builder AddRequires(Func<Task<string?>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncStringInfo<T>(description, expression));
        }

        public Builder AddRequires(Func<Task<bool>> expression, [CallerArgumentExpression(nameof(expression))] string? description = null)
        {
            ArgumentNullException.ThrowIfNull(description);
            return AddRequires(new RequiresAsyncBoolInfo<T>(description, expression));
        }

        public Target<T> Build()
        {
            if (Name == null)
            {
                throw new InvalidOperationException("Name cannot be null");
            }

            return new Target<T>(Name, Executes.ToArray(), DependsOn.ToArray(), After.ToArray(), Before.ToArray(), OnlyWhen.ToArray(), Requires.ToArray(), Unlisted);
        }

        public static implicit operator Target<T>(Builder builder)
        {
            return builder.Build();
        }
    }
}