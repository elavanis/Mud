using System.Collections.Generic;
using static Shared.FileIO.Interface.CachedThings.FileExits;

namespace Shared.FileIO.Interface
{
    public interface ICachedFileIO
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

        #region Other
        string[] GetFilesFromDirectory(string directory);
        Exists Exists(string fileName);
        #endregion Other


    }
}