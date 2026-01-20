namespace Bacon.Build;

public static class AbsolutePathExtensions
{
    public static bool FileExists(this AbsolutePath path)
    {
        return File.Exists(path);
    }

    public static bool DirectoryExists(this AbsolutePath path)
    {
        return Directory.Exists(path);
    }

    public static void DeleteFile(this AbsolutePath path)
    {
        if (path.FileExists())
        {
            File.Delete(path);
        }
    }

    public static void CreateDirectory(this AbsolutePath path)
    {
        Directory.CreateDirectory(path);
    }

    public static void DeleteDirectory(this AbsolutePath path, bool recursive = false)
    {
        if (path.DirectoryExists())
        {
            Directory.Delete(path, recursive);
        }
    }

    public static void EnsureDirectoryExists(this AbsolutePath path)
    {
        string p = path;
        if (!Directory.Exists(p))
        {
            Directory.CreateDirectory(p);
        }
    }

    public static void CreateOrCleanDirectory(this AbsolutePath path)
    {
        string p = path;
        if (Directory.Exists(p))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(p);
    }

    public static void CopyFile(this AbsolutePath from, AbsolutePath to)
    {
        File.Copy(from, to);
    }

    public static void CopyDirectory(this AbsolutePath from, AbsolutePath to)
    {
        to.EnsureDirectoryExists();

        var fromDirectory = new DirectoryInfo(from);

        foreach (var fsi in fromDirectory.EnumerateFileSystemInfos())
        {
            switch (fsi)
            {
                case FileInfo fi:
                    fi.CopyTo(to / fi.Name);
                    break;
                case DirectoryInfo di:
                    ((AbsolutePath)di.FullName).CopyDirectory(to / di.Name);
                    break;
            }
        }
    }
}