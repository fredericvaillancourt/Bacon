namespace Bacon.Build;

public readonly struct AbsolutePath : IEquatable<AbsolutePath>
{
    private readonly string _path;

    private AbsolutePath(string path)
    {
        if (!Path.IsPathFullyQualified(path))
        {
            throw new ArgumentException("Not a full path", nameof(path));
        }

        _path = Path.GetFullPath(path);
    }

    public AbsolutePath? Parent
    {
        get
        {
            string? path = Path.GetDirectoryName(_path);
            return path != null ? new(path) : null;
        }
    }

    public static AbsolutePath operator /(AbsolutePath path, RelativePath part)
    {
        return new AbsolutePath(Path.Combine(path._path, part));
    }

    public static implicit operator string(AbsolutePath path)
    {
        return path._path;
    }

    public static implicit operator AbsolutePath(string path)
    {
        return new AbsolutePath(path);
    }

    public bool Equals(AbsolutePath other)
    {
        return _path == other._path;
    }

    public override bool Equals(object? obj)
    {
        return obj is AbsolutePath other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _path.GetHashCode();
    }

    public override string ToString()
    {
        return _path;
    }
}