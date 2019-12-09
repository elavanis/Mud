using System.Collections.Generic;

namespace Shared.FileIO.Interface
{
    public interface ICachingFileIO
    {
        #region Read
        string ReadAllText(string fileName);

        string ReadFileBase64(string fileName);

        string[] ReadLines(string fileName);

        byte[] ReadBytes(string fileName);
        #endregion Read

        #region Write
        void WriteFile(string fileName, string file);

        void WriteFileBase64(string fileName, string file);

        void AppendFile(string fileName, IEnumerable<string> lines);

        void AppendFile(string fileName, string line);
        #endregion Write
    }
}