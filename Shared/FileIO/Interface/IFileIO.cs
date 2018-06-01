using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FileIO.Interface
{
    public interface IFileIO
    {
        void AppendFile(string fileName, IEnumerable<string> lines);

        void AppendFile(string fileName, string line);

        string ReadAllText(string fileName);

        string ReadFileBase64(string fileName);

        bool Exists(string fileName);

        IEnumerable<string> ReadLines(string fileName);

        void WriteFile(string fileName, string file);

        void WriteFileBase64(string fileName, string file);

        void EnsureDirectoryExists(string directory);

        string[] GetFilesFromDirectory(string directory);

        string[] GetFilesFromDirectory(string zoneLocation, string filter);

        byte[] ReadBytes(string fileName);
        void Delete(string file);
    }
}
