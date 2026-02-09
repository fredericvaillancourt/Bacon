namespace Bacon.Build;

public abstract class Arguments(Arguments.Builder builder)
{
    public IBuildOutput? BuildOutput { get; } = builder.BuildOutput;

    public string Format()
    {
        Span<char> buffer = stackalloc char[256]; //TODO: Should this be 128?
        var builder = new ArgumentsBuilder(buffer);
        try
        {
            DoFormat(ref builder);
            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
    }

    protected abstract void DoFormat(ref ArgumentsBuilder builder);

    public abstract class Builder
    {
        protected Builder()
        {
        }

        protected Builder(Arguments value)
        {
            BuildOutput = value.BuildOutput;
        }

        public IBuildOutput? BuildOutput { get; set;  }

        public Builder SetBuildOutput(IBuildOutput? value)
        {
            BuildOutput = value;
            return this;
        }

        public Builder ResetBuildOutput()
        {
            BuildOutput = null;
            return this;
        }
    }
}
//TODO: Should we allow/use derived attributes? That would allow user defined one ... but this means we have to use reflection to get them.