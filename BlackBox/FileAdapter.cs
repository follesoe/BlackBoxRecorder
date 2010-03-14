using System.IO;
using System.Xml.Linq;

namespace BlackBox
{
    public class FileAdapter : IFile
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path).FullName;
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public void Save(XDocument xml, string path)
        {
            xml.Save(path);            
        }
    }
}
