using System.Collections.Generic;

namespace Shared.FileIO.Interface
{
    internal interface IFileIO
    {
        void AppendFile(string fileName, IEnumerable<string> lines);

        void AppendFile(string fileName, string line);

        string ReadAllText(string fileName, bool useCache = true);

        string ReadFileBase64(string fileName, bool useCache = true);

        bool Exists(string fileName);

        IEnumerable<string> ReadLines(string fileName);

        void WriteFile(string fileName, string file);

        void WriteFileBase64(string fileName, string file);

        void EnsureDirectoryExists(string directory);

        string[] GetFilesFromDirectory(string directory);

        string[] GetFilesFromDirectory(string zoneLocation, string filter);

        byte[] ReadBytes(string fileName);

        void Delete(string file);

        void FlushCache();

    }
}
