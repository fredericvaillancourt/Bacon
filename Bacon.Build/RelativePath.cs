namespace Bacon.Build;

public readonly struct RelativePath : IEquatable<RelativePath>
{
    private readonly string _path;

    private RelativePath(string path)
    {
        //TODO: We need more validation than that ...
        if (Path.IsPathFullyQualified(path))
        {
            throw new ArgumentException("Not a relative path", nameof(path));
        }

        _path = path;
    }

    public static implicit operator string(RelativePath path)
    {
        return path._path;
    }

    public static implicit operator RelativePath(string path)
    {
        return new RelativePath(path);
    }

    public bool Equals(RelativePath other)
    {
        return _path == other._path;
    }

    public override bool Equals(object? obj)
    {
        return obj is RelativePath other && Equals(other);
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