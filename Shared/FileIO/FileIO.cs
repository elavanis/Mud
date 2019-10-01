using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Shared.FileIO
{
    public class FileIO : IFileIO
    {
        private object padLock = new object();
        private Dictionary<string, string> fileText = new Dictionary<string, string>();
        private Dictionary<string, string> base64 = new Dictionary<string, string>();

        [ExcludeFromCodeCoverage]
        public void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        [ExcludeFromCodeCoverage]
        public string[] GetFilesFromDirectory(string directory)
        {
            return Directory.GetFiles(directory);
        }

        [ExcludeFromCodeCoverage]
        public string[] GetFilesFromDirectory(string directory, string filter)
        {
            return Directory.GetFiles(directory, filter);
        }

        [ExcludeFromCodeCoverage]
        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        [ExcludeFromCodeCoverage]
        public IEnumerable<string> ReadLines(string fileName)
        {
            return File.ReadLines(fileName);
        }

        [ExcludeFromCodeCoverage]
        public string ReadAllText(string fileName, bool useCache = true)
        {
            if (!useCache)
            {
                return File.ReadAllText(fileName);
            }
            else
            {
                lock (padLock)
                {
                    string file = null;
                    //if the file is not in the cache the read the contents and add it
                    if (!fileText.TryGetValue(fileName, out file))
                    {
                        file = File.ReadAllText(fileName);
                        fileText.Add(fileName, file);
                    }

                    return file;
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public void WriteFile(string fileName, string file)
        {
            File.WriteAllText(fileName, file);
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

        [ExcludeFromCodeCoverage]
        public string ReadFileBase64(string fileName, bool useCache = true)
        {
            if (!useCache)
            {
                byte[] bytes = ReadBytes(fileName);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                lock (padLock)
                {
                    string file = null;
                    //if the file is not in the cache the read the contents and add it
                    if (!base64.TryGetValue(fileName, out file))
                    {
                        byte[] bytes = ReadBytes(fileName);
                        file = Convert.ToBase64String(bytes);
                        base64.Add(fileName, file);
                    }

                    return file;
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public void WriteFileBase64(string fileName, string file)
        {
            byte[] bytes = Convert.FromBase64String(file);
            File.WriteAllBytes(fileName, bytes);
        }

        [ExcludeFromCodeCoverage]
        public byte[] ReadBytes(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            return bytes;
        }

        [ExcludeFromCodeCoverage]
        public void Delete(string file)
        {
            File.Delete(file);
        }

        [ExcludeFromCodeCoverage]
        public void FlushCache()
        {

        }
    }
}
