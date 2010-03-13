using System.Xml.Linq;

namespace BlackBox
{
    public interface IFile
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);        
        string CreateDirectory(string path);
        void Save(XDocument xml, string path);
    }
}
