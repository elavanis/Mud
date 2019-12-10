using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Shared.FileIO
{
    public class FileIO : IFileIO
    {
        #region Read
        [ExcludeFromCodeCoverage]
        public string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        [ExcludeFromCodeCoverage]
        public string ReadFileBase64(string fileName)
        {
            byte[] bytes = ReadBytes(fileName);
            return Convert.ToBase64String(bytes);
        }

        [ExcludeFromCodeCoverage]
        public string[] ReadLines(string fileName)
        {
            return File.ReadLines(fileName).ToArray();
        }

        [ExcludeFromCodeCoverage]
        public byte[] ReadBytes(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            return bytes;
        }
        #endregion Read

        #region Write
        [ExcludeFromCodeCoverage]
        public void WriteFile(string fileName, string file)
        {
            File.WriteAllText(fileName, file);
        }

        [ExcludeFromCodeCoverage]
        public void WriteFileBase64(string fileName, string file)
        {
            byte[] bytes = Convert.FromBase64String(file);
            File.WriteAllBytes(fileName, bytes);
        }

        [ExcludeFromCodeCoverage]
        public void AppendFile(string fileName, IEnumerable<string> lines)
        {
            File.AppendAllLines(fileName, lines);
        }

        [ExcludeFromCodeCoverage]
        public void AppendFile(string fileName, string line)
        {
            File.AppendAllText(fileName, line);
        }
        #endregion Write

        #region Other
        [ExcludeFromCodeCoverage]
        public string[] GetFilesFromDirectory(string directory)
        {
            return Directory.GetFiles(directory);
        }

        [ExcludeFromCodeCoverage]
        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }
        #endregion Other

    }
}
