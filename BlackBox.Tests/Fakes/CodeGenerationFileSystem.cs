using System;
using System.Xml.Linq;

namespace BlackBox.Tests.Fakes
{
    public class CodeGenerationFileSystem : IFile
    {
        public string GeneratedCode;
        public string CodeSavedToPath;

        public void Save(string testClass, string path)
        {
            GeneratedCode = testClass;
            CodeSavedToPath = path;
        }

        #region Unimplemented members
        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public string CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public void Save(XDocument xml, string path)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
