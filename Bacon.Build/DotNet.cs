namespace Bacon.Build;

public sealed partial class DotNet(Context context)
{
    private static readonly object Key = new();

    public ITool<string, Result> Tool
    {
        get => context.GetOrAdd(Key, static ctx => ctx.SearchForCommand("dotnet"));
        set => context.Set(Key, value);
    }
}

public static class DotNetExtensions
{
    extension(Context self)
    {
        public DotNet DotNet => new(self);
    }
}
