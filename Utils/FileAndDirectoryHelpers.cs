﻿#nullable enable
using Utils;

public static class FileAndDirectoryHelpers
{
    public static string ChangeExtension(this string file, string extension)
    {
        return Path.ChangeExtension(file, extension);
    }

    public static bool HasExtension(this string filename, string extension)
    {
        return Path.HasExtension(filename) && Path.GetExtension(filename).IsSame(extension);
    }

    public const string LoadFolder = "Rogue";

    public static string CreateLoadFolder(string folder)
    {
        var fqn = GetLoadDirectory(folder);

        CreateFolder(fqn);

        return fqn;
    }

    public static void CreateFolder(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public static string GetLoadDirectory(string folder)
    {
        var path = Directory.GetCurrentDirectory();

        var fullyQualified = Path.Combine(path, folder);

        return fullyQualified;
    }

    public static string GetFullQualifiedName(params string[] paths)
    {
        var current = Directory.GetCurrentDirectory();

        var combined = new List<string> { current };
        combined.AddRange(paths);

        var fullyQualified = Path.Combine(combined.ToArray());
        return fullyQualified;
    }
}
