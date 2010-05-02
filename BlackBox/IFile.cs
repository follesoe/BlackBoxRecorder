using System.Xml.Linq;

namespace BlackBox
{
    public interface IFile
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);        
        string CreateDirectory(string path);
        string GetCurrentDirectory();
        void Save(XDocument xml, string path);
        void Save(string testClass, string path);
    }
}
