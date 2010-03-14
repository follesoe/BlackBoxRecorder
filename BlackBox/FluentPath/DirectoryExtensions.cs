using System.IO;
using System.Linq;

namespace BlackBox.FluentPath
{
    public static class DirectoryExtensions
    {
        public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOptions)
        {
            var directory = new DirectoryInfo(path);
            DirectoryInfo[] directories = directory.GetDirectories("*", searchOptions);

            var query = from d in directories
                        from item in Directory.GetFileSystemEntries(d.FullName, searchPattern)
                        select item;

            return query.ToArray();
        }
    }
}
