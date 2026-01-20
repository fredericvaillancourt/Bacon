namespace Bacon.Build;

public static class FileSearch
{
    public static string? SearchPath(string fileName)
    {
        string[]? paths = GetPaths();
        if (paths == null)
        {
            return null;
        }

        return Search(paths, fileName);
    }

    public static string? SearchPath(ReadOnlySpan<string> fileNames)
    {
        string[]? paths = GetPaths();
        if (paths == null)
        {
            return null;
        }

        return Search(paths, fileNames);
    }

    public static string? Search(ReadOnlySpan<string> paths, ReadOnlySpan<string> fileNames)
    {
        foreach (string path in paths)
        {
            string? result = Search(path, fileNames);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public static string? Search(ReadOnlySpan<string> paths, string fileName)
    {
        foreach (string path in paths)
        {
            string? result = Search(path, fileName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public static string? Search(string path, ReadOnlySpan<string> fileNames)
    {
        foreach (string fileName in fileNames)
        {
            string? result = Search(path, fileName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public static string? Search(string path, string fileName)
    {
        string fullPath = Path.Combine(path, fileName);
        return File.Exists(fullPath) ? fullPath : null;
    }

    private static string[]? GetPaths()
    {
        return Environment.GetEnvironmentVariable("PATH")?.Split(OperatingSystem.IsWindows() ? ';' : ':');
    }
}