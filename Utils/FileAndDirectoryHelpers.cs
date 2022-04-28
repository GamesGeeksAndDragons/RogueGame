#nullable enable
using System.IO;

namespace Utils
{
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
            if (!Directory.Exists(fqn))
            {
                Directory.CreateDirectory(fqn);
            }

            return fqn;
        }

        public static string GetLoadDirectory(string folder)
        {
            var path = Directory.GetCurrentDirectory();

            var fullyQualified = Path.Combine(path, folder);

            return fullyQualified;
        }

        public static string GetFullQualifiedName(params string[] paths)
        {
            var path = Directory.GetCurrentDirectory();
            var combined = new List<string>
            {
                path
            };
            combined.AddRange(paths);

            var fullyQualified = Path.Combine(combined.ToArray());
            return fullyQualified;
        }
    }
}
