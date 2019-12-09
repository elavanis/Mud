using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shared.FileIO
{
    public class CachedFileIO : ICachingFileIO
    {
        private Dictionary<string, CachedFile> CachedFiles = new Dictionary<string, CachedFile>();
        private FileIO FileIO = new FileIO();

        #region Write
        public void AppendFile(string fileName, IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                AppendFile(fileName, line);
            }
        }

        public void AppendFile(string fileName, string line)
        {
            AppendStream(fileName, Encoding.UTF8.GetBytes(line + Environment.NewLine));
        }

        public void WriteFile(string fileName, string file)
        {
            WriteStream(fileName, Encoding.UTF8.GetBytes(file + Environment.NewLine));
        }

        public void WriteFileBase64(string fileName, string file)
        {
            byte[] bytes = Convert.FromBase64String(file + Environment.NewLine);
            WriteStream(fileName, bytes);
        }
        #endregion Write

        #region Read
        public string ReadAllText(string fileName)
        {
            return Encoding.UTF8.GetString(ReadStream(fileName));
        }

        public byte[] ReadBytes(string fileName)
        {
            return ReadStream(fileName);
        }

        public string ReadFileBase64(string fileName)
        {
            return Convert.ToBase64String(ReadStream(fileName));
        }

        public string[] ReadLines(string fileName)
        {
            return ReadAllText(fileName).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
        #endregion Read


        private CachedFile GetStream(string fileName)
        {
            CachedFile cachedFile = null;
            lock (CachedFiles)
            {
                CachedFiles.TryGetValue(fileName, out cachedFile);

                if (cachedFile == null)
                {
                    cachedFile = new CachedFile(fileName);
                    CachedFiles.Add(fileName, cachedFile);
                }
            }

            return cachedFile;
        }

        private MemoryStream LoadFromFile(string fileName)
        {
            MemoryStream memoryStream = new MemoryStream();
            if (FileIO.Exists(fileName))
            {
                byte[] bytes = FileIO.ReadBytes(fileName);
                memoryStream.Write(bytes, 0, bytes.Length);
            }
            return memoryStream;
        }

        private byte[] ReadStream(string fileName)
        {
            CachedFile cachedFile = GetStream(fileName);
            lock (cachedFile)
            {
                if (cachedFile.MemoryStream == null)
                {
                    cachedFile.MemoryStream = LoadFromFile(fileName);
                }

                cachedFile.LastAccessed = DateTime.Now;

                return cachedFile.MemoryStream.ToArray();
            }
        }

        private void AppendStream(string fileName, byte[] dataToWrite)
        {
            CachedFile cachedFile = GetStream(fileName);
            lock (cachedFile)
            {
                if (cachedFile.MemoryStream == null)
                {
                    cachedFile.MemoryStream = LoadFromFile(fileName);
                }

                cachedFile.LastAccessed = DateTime.Now;
                cachedFile.MemoryStream.Seek(0, SeekOrigin.End);
                cachedFile.MemoryStream.Write(dataToWrite, 0, dataToWrite.Length);
            }
        }

        private void WriteStream(string fileName, byte[] dataToWrite)
        {
            CachedFile cachedFile = GetStream(fileName);
            lock (cachedFile)
            {
                if (cachedFile.MemoryStream == null)
                {
                    cachedFile.MemoryStream = LoadFromFile(fileName);
                }

                cachedFile.LastAccessed = DateTime.Now;
                cachedFile.MemoryStream = new MemoryStream(dataToWrite);
            }
        }
    }
}
